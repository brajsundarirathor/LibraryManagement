using AutoMapper;
//using LibraryManagement.Migrations;
using LibraryManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Identity.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;

namespace LibraryManagement.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BookDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
 

        public AuthRepository(BookDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            UserMaster userDetail = new UserMaster();
            var response = new ServiceResponse<string>();
            var user = await _context.UserMaster.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswaordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else
            {
                if (user != null)
                {
                    userDetail = new UserMaster
                    {
                        Username = user.Username,
                        UserId = user.UserId,
                        Role = user.Role
                    };
                }
                response.Data = CreateToken(user);
            }

            return response;
        }

        
        public async Task<ServiceResponse<int>> Register(UserMaster userMaster, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await UserExists(userMaster.Username))
            {
                response.Success = false;
                response.Message = "User already exists please login";
            }
            CeratePasswordHash(password, out byte[] passwaordHash, out byte[] passwordSalt);
            userMaster.PasswordHash = passwaordHash;
            userMaster.PasswordSalt = passwordSalt;

            _context.UserMaster.Add(userMaster);
            await _context.SaveChangesAsync();

            response.Data = userMaster.UserId;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.UserMaster.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }

            return false;
        }


        private void CeratePasswordHash(string password, out byte[] passwaordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwaordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswaordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;

            }
        }

        private string CreateToken(UserMaster user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var claims = new[]
            {               
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> UploadImage1(IFormFile file)
        {
            var special = Guid.NewGuid().ToString();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserImg", special + "-" + file.FileName);
            using (FileStream ms = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(ms);
            }
            var filename = special + "-" + file.FileName;
            return filepath;
        }

    }
}

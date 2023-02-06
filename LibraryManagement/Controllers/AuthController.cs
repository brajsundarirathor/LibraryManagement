using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authrepo;
        private readonly BookDbContext _context;

        public AuthController(IAuthRepository authrepo, BookDbContext context)
        {
            _authrepo = authrepo;
            _context = context;
        }

        [HttpGet]
        [Route("Admins")]
        [Authorize(Policy = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }
        private UserMaster GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserMaster
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody]UserRegisterDto request)
        {
            var response = await _authrepo.Register(new UserMaster {FirstName=request.FirstName, LastName=request.LastName, Gender=request.Gender, Username = request.Username, Email = request.Email, UserPhoto = request.UserImg }, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login([FromBody]UserLoginDto request)
        {
            
            var response = await _authrepo.Login(request.username, request.password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Image Upload For Users")]
        public async Task<ActionResult<ServiceResponse<string>>> PostImage([FromForm]Image user)
        {
            //var reponse = await _authrepo.UploadImage1(user.)
            string path = await _authrepo.UploadImage1(user.FileUrl);
            var u = user.FileUrl.ToString();
            //await _context.UserMaster.AddAsync();
            await _context.SaveChangesAsync();
            return Ok(path);
        }      
    }
}

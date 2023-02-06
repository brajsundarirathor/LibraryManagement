using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(UserMaster userMaster, string password);

        Task<ServiceResponse<string>> Login(string username, string password);

        Task<bool> UserExists(string username);

        Task<string> UploadImage1(IFormFile file);
    }
}

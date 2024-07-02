using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AuthService.MVC.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
    }
}

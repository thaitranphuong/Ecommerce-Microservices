using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthService.MVC.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder = "test_cloudinary")
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = $"{folder}/{file.FileName}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }

}

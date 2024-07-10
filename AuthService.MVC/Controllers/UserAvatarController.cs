using AuthService.MVC.AsyncServices;
using AuthService.MVC.Constants;
using AuthService.MVC.Dtos;
using AuthService.MVC.Models;
using AuthService.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAvatarController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMessageProducer _messageProducer;

        public UserAvatarController(
            UserManager<AppUser> userManager,
            ICloudinaryService cloudinaryService,
            IMessageProducer messageProducer)
        {
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            var filePath = ""; ;
            foreach (IFormFile file in Request.Form.Files)
            {
                filePath = await _cloudinaryService.UploadFileAsync(file, "avatar");

                if (string.IsNullOrEmpty(filePath))
                {
                    return StatusCode(500, "An error occurred while uploading the file.");
                }
            }

            // Cập nhật avatar của người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            user.Avatar = filePath;
            var result = await _userManager.UpdateAsync(user);
            _messageProducer.SendMessage<UserPublishDto>(EventType.CreateUser, new UserPublishDto()
                                                                               {
                                                                                   Id = user.Id,
                                                                                   Avatar = user.Avatar,
                                                                                   UserName = user.UserName
                                                                               });

            if (!result.Succeeded)
            {
                return StatusCode(500, "Error updating user avatar");
            }

            return Ok(new { FilePath = filePath });
        }

        [HttpPost]
        [Route("/editor-upload")]
        public async Task<IActionResult> CloudUpload()
        {
            var filePath = ""; ;
            foreach (IFormFile file in Request.Form.Files)
            {
                filePath = await _cloudinaryService.UploadFileAsync(file, "editor");

                if (string.IsNullOrEmpty(filePath))
                {
                    return StatusCode(500, "An error occurred while uploading the file.");
                }
            }
            return Ok(new { url = filePath });
        }
    }

}

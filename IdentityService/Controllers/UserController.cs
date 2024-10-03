using IdentityService.Models.Dtos.Pagination;
using IdentityService.Models.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowAll")]
    //[Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileStorageService _fileStorageService;
        public UserController(IUserService userService, IFileStorageService fileStorageService) 
        {
            _userService = userService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(UserDto dto)
        {
            var result = await _userService.Create(dto);
            if (result != null) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpPost]
        [Route("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile image)
        {
            var filePath = await _fileStorageService.Upload("avatar", image);
            if (filePath != null) return Ok(new { path = filePath });
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            UserDto dto = await _userService.FindById(userId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(UserDto dto)
        {
            var result = await _userService.Update(dto);
            if (result != null) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<UserOutput>> GetAll(string email, int page = 1, int limit = 5)
        {
            return Ok(await _userService.FindAll(email, page, limit));
        }

        [HttpGet("get-all-nopagination")]
        public async Task<ActionResult<List<UserDto>>> GetAllNoPagination()
        {
            return Ok(await _userService.FindAllNoPagination());
        }

        [HttpGet("get-all-customer")]
        public async Task<ActionResult<List<UserDto>>> GetAllCustomer()
        {
            return Ok(await _userService.FindAllCustomer());
        }

        [HttpGet("get-all-admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllAdmin()
        {
            return Ok(await _userService.FindAllAdmin());
        }

        [HttpGet]
        [Route("showhide/{id}")]
        public async Task<IActionResult> ShowHide(string id)
        {
            int result = await _userService.SaveShowHide(id);
            if (result > 0) return Ok(result);
            else return StatusCode(500);
        }
    }
}

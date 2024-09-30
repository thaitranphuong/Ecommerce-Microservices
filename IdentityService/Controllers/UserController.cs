using IdentityService.Models.Dtos.Pagination;
using IdentityService.Models.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
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
    }
}

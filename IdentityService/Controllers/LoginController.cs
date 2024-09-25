using IdentityService.JWT;
using IdentityService.Models.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public LoginController(JwtTokenService jwtTokenService, IUserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            var dto = await _userService.Create(user);

            if(dto == null) 
                return BadRequest(dto);

            var token = _jwtTokenService.GenerateToken(user.Email, dto.Roles);
            return Ok(new { user = dto, Token = token });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto user)
        {
            var dto = await _userService.Login(user.Email, user.Password);

            if (dto == null)
                return BadRequest(dto);

            var token = _jwtTokenService.GenerateToken(user.Email, dto.Roles);
            return Ok(new { user = dto, Token = token });
        }
    }
}

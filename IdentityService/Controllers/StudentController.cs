using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;

        public StudentController(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;

        }

        [HttpGet]
        [Route("TestAllRoles")]
        [Authorize]
        public async Task<ActionResult> TestAllRoles()
        {
            return Ok();
        }

        [HttpGet]
        [Route("TestAdminRole")]
        [Authorize(Roles = "admin,AAA")]
        public async Task<ActionResult> TestAdminRole()
        {
            return Ok();
        }
        
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            return Ok("API Gateway is working");
        }
    }
}

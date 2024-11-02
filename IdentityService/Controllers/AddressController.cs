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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IFileStorageService _fileStorageService;
        public AddressController(IAddressService addressService, IFileStorageService fileStorageService) 
        {
            _addressService = addressService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(AddressDto dto)
        {
            var result = await _addressService.Create(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{addressId}")]
        public async Task<IActionResult> Get(int addressId)
        {
            AddressDto dto = await _addressService.FindById(addressId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(AddressDto dto)
        {
            var result = await _addressService.Update(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<AddressDto>>> GetAll(string userId)
        {
            return Ok(await _addressService.FindAllByUserId(userId));
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<List<AddressDto>>> Delete(int id)
        {
            var result = await _addressService.Remove(id);
            if (result) return Ok(new {status = 200});
            return StatusCode(500);
        }
    }
}

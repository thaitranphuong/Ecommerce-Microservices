﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryService.Dtos;
using InventoryService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(SupplierDto dto)
        {
            bool result = await _supplierService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{supplierId}")]
        public async Task<IActionResult> Get(int supplierId)
        {
            SupplierDto dto = await _supplierService.FindById(supplierId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _supplierService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(SupplierDto dto)
        {
            bool result = await _supplierService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{supplierId}")]
        public async Task<IActionResult> Delete(int supplierId)
        {
            bool result = await _supplierService.DeleteById(supplierId);
            if (result) return Ok(new { status = 200 });
            else return StatusCode(500);
        }
    }
}

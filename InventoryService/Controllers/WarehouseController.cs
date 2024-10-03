using Microsoft.AspNetCore.Http;
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
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(WarehouseDto dto)
        {
            bool result = await _warehouseService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{warehouseId}")]
        public async Task<IActionResult> Get(int warehouseId)
        {
            WarehouseDto dto = await _warehouseService.FindById(warehouseId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _warehouseService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(WarehouseDto dto)
        {
            bool result = await _warehouseService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{warehouseId}")]
        public async Task<IActionResult> Delete(int warehouseId)
        {
            bool result = await _warehouseService.DeleteById(warehouseId);
            if (result) return Ok(new { status = 200 });
            else return StatusCode(500);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Dtos;
using ProductService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(UnitDto dto)
        {
            bool result = await _unitService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{unitId}")]
        public async Task<IActionResult> Get(int unitId)
        {
            UnitDto dto = await _unitService.FindById(unitId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _unitService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(UnitDto dto)
        {
            bool result = await _unitService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{unitId}")]
        public async Task<IActionResult> Delete(int unitId)
        {
            bool result = await _unitService.DeleteById(unitId);
            if (result) return Ok(new {status = 200});
            else return StatusCode(500);
        }
    }
}

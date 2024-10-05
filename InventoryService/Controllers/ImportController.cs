using Microsoft.AspNetCore.Mvc;
using InventoryService.Dtos;
using InventoryService.Services;
using System.Threading.Tasks;
using System;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(ImportDto dto)
        {
            bool result = await _importService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{importId}")]
        public async Task<IActionResult> Get(int importId)
        {
            ImportDto dto = await _importService.FindById(importId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit,
                                                [FromQuery] DateTime startTime,
                                                [FromQuery] DateTime endTime)
        {
            var dtos = await _importService.FindAll(startTime, endTime, page, limit);
            return Ok(dtos);
        }
    }
}

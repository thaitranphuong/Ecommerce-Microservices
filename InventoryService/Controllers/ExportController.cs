using Microsoft.AspNetCore.Mvc;
using InventoryService.Dtos;
using InventoryService.Services;
using System.Threading.Tasks;
using System;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;

        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(ExportDto dto)
        {
            bool result = await _exportService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{exportId}")]
        public async Task<IActionResult> Get(int exportId)
        {
            ExportDto dto = await _exportService.FindById(exportId);
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
            var dtos = await _exportService.FindAll(startTime, endTime, page, limit);
            return Ok(dtos);
        }
    }
}

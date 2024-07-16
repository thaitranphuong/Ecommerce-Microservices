using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Services;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(VoucherDto dto)
        {
            bool result = await _voucherService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{voucherId}")]
        public async Task<IActionResult> Get(int voucherId)
        {
            VoucherDto dto = await _voucherService.FindById(voucherId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _voucherService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("get-all-of-customer-page")]
        public async Task<IActionResult> GetAllOfCustomerPage([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _voucherService.FindAllOfCustomerPage(name, page, limit);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(VoucherDto dto)
        {
            bool result = await _voucherService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{voucherId}")]
        public async Task<IActionResult> Delete(int voucherId)
        {
            bool result = await _voucherService.DeleteById(voucherId);
            if (result) return Ok();
            else return StatusCode(500);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Services;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(OrderDto dto)
        {
            bool result = await _orderService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            OrderDto dto = await _orderService.FindById(userId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] OrderStatus status)
        {
            var dtos = await _orderService.FindAll(status, page, limit);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("get-all-of-customer-page")]
        public async Task<IActionResult> GetAllOfCustomerPage([FromQuery] OrderStatus status, [FromQuery] string userId)
        {
            var dtos = await _orderService.FindAllByUserId(userId, status);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(OrderDto dto)
        {
            bool result = await _orderService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }
    }
}

using CartService.Dtos;
using CartService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CartItemDto dto)
        {
            var result = await _cartItemService.Add(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get-all/{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            var result = await _cartItemService.FindByUserId(userId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(CartItemDto dto)
        {
            var result = await _cartItemService.Update(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{userId}/{productId}")]
        public async Task<IActionResult> Delete(string userId, int productId)
        {
            var result = await _cartItemService.Delete(userId, productId);
            if (result) return Ok();
            else return StatusCode(500);
        }
    }
}

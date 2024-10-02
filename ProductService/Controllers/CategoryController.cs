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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            bool result = await _categoryService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{categoryId}")]
        public async Task<IActionResult> Get(int categoryId)
        {
            CategoryDto dto = await _categoryService.FindById(categoryId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _categoryService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            bool result = await _categoryService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            bool result = await _categoryService.DeleteById(categoryId);
            if (result) return Ok(new {status = 200});
            else return StatusCode(500);
        }
    }
}

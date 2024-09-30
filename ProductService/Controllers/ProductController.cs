using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileStorageService _fileStorageService;

        public ProductController(IProductService productService, IFileStorageService fileStorageService)
        {
            _productService = productService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(IFormFile image)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["product"];

            var product = JsonConvert.DeserializeObject<ProductDto>(json);
            if (image != null)
            {
                var filePath = await _fileStorageService.Upload("product", image);
                product.Thumbnail = filePath;
            }
            int result = await _productService.Save(product);
            if (result > 0) return Ok(result);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ProductDto dto = await _productService.FindById(id);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string name)
        {
            var dtos = await _productService.FindAll(name, page, limit);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("get-all-customer")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit,
                                                [FromQuery] string name,
                                                [FromQuery] int categoryId,
                                                [FromQuery] float price)
        {
            var dtos = await _productService.FindAll(name, categoryId, price, page, limit);
            return Ok(dtos);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(IFormFile image)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["product"];
            var product = JsonConvert.DeserializeObject<ProductDto>(json);
            if (image != null)
            {
                var filePath = await _fileStorageService.Upload("product", image);
                product.Thumbnail = filePath;
            }
            int result = await _productService.Save(product);
            if (result > 0) return Ok(result);
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("showhide/{id}")]
        public async Task<IActionResult> ShowHide(int id)
        {
            int result = await _productService.SaveShowHide(id);
            if (result > 0) return Ok(result);
            else return StatusCode(500);
        }
    }
}

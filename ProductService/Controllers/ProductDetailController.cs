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
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        private readonly IFileStorageService _fileStorageService;

        public ProductDetailController(IProductDetailService productDetailService, IFileStorageService fileStorageService)
        {
            _productDetailService = productDetailService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(IFormFile[] productDetailImages)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["productDetails"];

            var productDetails = JsonConvert.DeserializeObject<List<ProductDetailDto>>(json);
            var result = false;
            for (int i = 0; i < productDetailImages.Length; i++)
            {
                var filePath = await _fileStorageService.Upload("productDetail", productDetailImages[i]);
                productDetails[i].Image = filePath;
                result = await _productDetailService.Save(productDetails[i]);
            }
            if (result) return Ok();
            else return StatusCode(500);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(int[] deletedProductDetailIds)
        {
            foreach (var id in deletedProductDetailIds) {
                await _productDetailService.DeleteById(id);
            }
            return Ok();
        }
    }
}

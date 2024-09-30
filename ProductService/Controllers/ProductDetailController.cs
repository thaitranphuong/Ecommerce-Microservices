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
        public async Task<IActionResult> Create(IFormFile productDetailImage)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["productDetail"];

            var productDetail = JsonConvert.DeserializeObject<ProductDetailDto>(json);
            var filePath = await _fileStorageService.Upload("productDetail", productDetailImage);
            productDetail.Image = filePath;
            await _productDetailService.Save(productDetail);
            return Ok(new { status = 200});
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(int[] deletedProductDetailIds)
        {
            foreach (var id in deletedProductDetailIds) {
                await _productDetailService.DeleteById(id);
            }
            return Ok(new { statusCode = 200 });
        }
    }
}

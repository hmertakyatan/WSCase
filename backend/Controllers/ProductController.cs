using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync();
                if (result == null || result.Data == null || !result.Data.Any())
                {
                    return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessMessage(HttpContext, "Kategoriye ait ürün bulunamadı.", data: []));
                }
                var response = ApiResponse<IEnumerable<ProductResponse>>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var result = await _productService.GetProductByIdAsync(id);
                var response = ApiResponse<ProductResponse>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest newProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext,"Geçersiz ürün verisi.", 400));

            try
            {
                var result = await _productService.AddProductAsync(newProduct);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.",500));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductRequest updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Geçersiz ürün verisi.", 400));

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "IP adresi alınamadı.", 400));

            try
            {
                var result = await _productService.UpdateProductAsync(id, updatedProduct, ipAddress);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPost("by-categories")]
        public async Task<IActionResult> GetProductsByCategoryIds([FromBody] List<Guid> categories)
        {
            if (categories == null || !categories.Any())
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Kategori ID'leri boş veya eksik.", 400));

            try
            {
                var result = await _productService.GetProductsByCategoryIdsAsync(categories);
                var response = ApiResponse<IEnumerable<ProductResponse>>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPost("as-list")]
        public async Task<IActionResult> AddProductsAsList([FromBody] List<ProductRequest> products)
        {
            if (products == null || !products.Any())
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Ürün listesi boş.", 400));

            try
            {
                var result = await _productService.AddProductsRangeAsync(products);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }
    }
}
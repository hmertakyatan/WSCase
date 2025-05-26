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
                var products = await _productService.GetAllProductsAsync();
                if (products == null || !products.Any())
                    return Ok(ApiResponse<IEnumerable<ProductResponse>>.SuccessMessage("Ürün bulunamadı.", data: []));

                return Ok(ApiResponse<IEnumerable<ProductResponse>>.SuccessMessage("Tüm ürünler başarıyla getirildi.", data: products));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
   
                return Ok(ApiResponse<ProductResponse>.SuccessMessage("Ürün başarıyla getirildi.", data: product));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest newProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage("Geçersiz ürün verisi."));

            try
            {
                var result = await _productService.AddProductAsync(newProduct);
                if (!result)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Ürün eklenemedi.", statusCode:404));

                return Ok(ApiResponse<bool>.SuccessMessage("Ürün başarıyla oluşturuldu."));
            }
            catch(DatabaseOperationException ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductRequest updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage("Geçersiz ürün verisi."));

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
                return BadRequest(ApiResponse<object>.ErrorMessage("IP adresi alınamadı."));

            try
            {
                var result = await _productService.UpdateProductAsync(id, updatedProduct, ipAddress);
                if (!result)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Ürün güncellenemedi."));

                return Ok(ApiResponse<bool>.SuccessMessage("Ürün başarıyla güncellendi."));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch(DatabaseOperationException ex)
            {
                return StatusCode(500,ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 500));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Ürün silinemedi."));

                return Ok(ApiResponse<bool>.SuccessMessage("Ürün başarıyla silindi.", data: true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorMessage(ex.Message, statusCode:404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpPost("by-categories")]
        public async Task<IActionResult> GetProductsByCategoryIds([FromBody] List<Guid> categories)
        {
            if (categories == null || !categories.Any())
                return BadRequest(ApiResponse<object>.ErrorMessage("Kategori ID'leri boş veya eksik.", statusCode: 400));

            try
            {
                var products = await _productService.GetProductsByCategoryIdsAsync(categories);
                if (products == null || !products.Any())
                    return Ok(ApiResponse<IEnumerable<ProductResponse>>.SuccessMessage("Kategoriye ait ürün bulunamadı.", data: []));

                return Ok(ApiResponse<IEnumerable<ProductResponse>>.SuccessMessage("Ürünler başarıyla getirildi.", data: products));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpPost("as-list")]
        public async Task<IActionResult> AddProductsAsList([FromBody] List<ProductRequest> products)
        {
            if (products == null || !products.Any())
                return BadRequest(ApiResponse<object>.ErrorMessage("Ürün listesi boş veya null."));

            try
            {
                var success = await _productService.AddProductsRangeAsync(products);
                if (!success)
                    return StatusCode(500, ApiResponse<object>.ErrorMessage("Ürünler eklenirken hata oluştu."));

                return Ok(ApiResponse<bool>.SuccessMessage("Ürünler başarıyla eklendi.", data: true));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}

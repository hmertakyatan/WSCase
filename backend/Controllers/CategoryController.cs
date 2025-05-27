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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoryService.GetAllCategoriesAsync();
                if (result == null || result.Data == null || !result.Data.Any())
                {
                    return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessMessage(HttpContext, "Kategoriye ait ürün bulunamadı.", data: []));
                }

                var response = ApiResponse<IEnumerable<CategoryResponse>>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var result = await _categoryService.GetCategoryByIdAsync(id);
                var response = ApiResponse<CategoryResponse>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest newCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Geçersiz kategori verisi.", 400));

            try
            {
                var result = await _categoryService.AddCategoryAsync(newCategory);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryRequest updatedCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Geçersiz kategori verisi.", 400));

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "IP adresi alınamadı.", 400));

            try
            {
                var result = await _categoryService.UpdateCategoryAsync(id, updatedCategory, ipAddress);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }
        [HttpPost("as-list")]
        public async Task<IActionResult> AddProductsAsList([FromBody] List<CategoryRequest> categories)
        {
            if (categories == null || !categories.Any())
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Kategori listesi boş.", 400));

            try
            {
                var result = await _categoryService.AddCategoriesRangeAsync(categories);
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

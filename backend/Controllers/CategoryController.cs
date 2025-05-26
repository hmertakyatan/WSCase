using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Controllers
{   [Authorize]
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
                var categories = await _categoryService.GetAllCategoriesAsync();
                if (categories == null || !categories.Any())
                    return Ok(ApiResponse<IEnumerable<ProductResponse>>.SuccessMessage("Kategoriye ait ürün bulunamadı.", data: []));
                return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessMessage("Tüm kategoriler başarıyla getirildi.", data: categories, statusCode: 200));
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
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var response = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(ApiResponse<CategoryResponse>.SuccessMessage("Kategori başarıyla getirildi.", data: response));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu.", statusCode:500));
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest newCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Geçersiz kategori verisi.", statusCode: 400));
                var result = await _categoryService.AddCategoryAsync(newCategory);
                return Ok(ApiResponse<bool>.SuccessMessage("Kategori başarıyla oluşturuldu."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch(Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu.", statusCode:500));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryRequest updatedCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Geçersiz kategori verisi.", statusCode: 400));
                var remoteIp = HttpContext.Connection.RemoteIpAddress;
                if (remoteIp == null)
                    return BadRequest(ApiResponse<string>.ErrorMessage("IP adresi alınamadı."));
                var result = await _categoryService.UpdateCategoryAsync(id, updatedCategory, remoteIp.ToString());
                return Ok(ApiResponse<bool>.SuccessMessage("Kategori başarıyla güncellendi.", data: result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch (DatabaseOperationException ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorMessage(ex.Message, statusCode: 500));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu.", statusCode:500));
            }
            
            
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var response = await _categoryService.DeleteCategoryAsync(id);
                return Ok(ApiResponse<bool>.SuccessMessage("Kategori başarıyla silindi", data: true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorMessage(ex.Message, statusCode: 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorMessage("Beklenmeyen bir hata oluştu.", statusCode: 500));
            }
            

        }
    }
}
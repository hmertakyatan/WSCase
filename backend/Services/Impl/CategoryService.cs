using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Repository.Interfaces.IRepository;
using WorkSoftCase.Services.Interfaces;
using WorkSoftCase.Services.Results;

namespace WorkSoftCase.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Result<object>> AddCategoryAsync(CategoryRequest newCategory)
        {
            var category = new Category
            {
                CategoryName = newCategory.CategoryName,
                CategoryIcon = newCategory.CategoryIcon,
                CreateDate = DateTime.UtcNow
            };
            var success = await _categoryRepository.AddAsync(category);
            if (!success)
                return Result<object>.Failure(message: "Kategori oluşturulamadı.");

            return Result<object>.Success(message: "Kategori başarıyla oluşturuldu.");
        }

        public async Task<Result<object>> DeleteCategoryAsync(Guid id)
        {
            var success = await _categoryRepository.DeleteAsync(id);
            if (success!)
                return Result<object>.Failure(message: "Kategori silinemedi.");
            return Result<object>.Success(message: "Kategori başarıyla silindi.");
        }

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryResponseList = categories.Select(p => new CategoryResponse
            {
                Id = p.Id,
                CategoryName = p.CategoryName,
                CategoryIcon = p.CategoryIcon,
            });
            return Result<IEnumerable<CategoryResponse>>.Success(message: "Kategoriler başarıyla getirildi.", data: categoryResponseList);
        }

        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return Result<CategoryResponse>.Failure(message: "Kategori bulunamadı.", statusCode: 404);
            var categoryResponse = new CategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryIcon = category.CategoryIcon
            };
            return Result<CategoryResponse>.Success(message: "Kategori başarıyla getirildi.", data:categoryResponse);
        }

        public async Task<Result<object>> UpdateCategoryAsync(Guid id, CategoryRequest request, String ipAddress)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return Result<object>.Failure(message: "Kategori bulunamadı", statusCode: 404);
            existingCategory.CategoryName = request.CategoryName;
            existingCategory.CategoryIcon = request.CategoryIcon;
            existingCategory.ModifyDate = DateTime.UtcNow;
            existingCategory.ModifyIP = ipAddress;
            var success = await _categoryRepository.UpdateAsync(existingCategory);
            if (!success)
                return Result<object>.Failure(message: "Kategori güncellenemedi.");
            return Result<object>.Success(message: "Kategori başarıyla güncellendi.");
        }

        public async Task<Result<object>> AddCategoriesRangeAsync(IEnumerable<CategoryRequest> categoryRequests)
        {
            var categories = categoryRequests.Select(c => new Category
            {
                CategoryName = c.CategoryName,
                CategoryIcon = c.CategoryIcon,
                CreateDate = DateTime.UtcNow,
            });

            var success = await _categoryRepository.AddRangeAsync(categories);
            if (!success)
                return Result<object>.Failure(message: "Kategoriler eklenemedi.");

            return Result<object>.Success(message: "Kategoriler başarıyla oluşturuldu.");
        }
    }
}
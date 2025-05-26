using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Repository.Interfaces.IRepository;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> AddCategoryAsync(CategoryRequest newCategory)
        {
            var category = new Category
            {
                CategoryName = newCategory.CategoryName,
                CategoryIcon = newCategory.CategoryIcon,
                CreateDate = DateTime.UtcNow
            };
            var success = await _categoryRepository.AddAsync(category);
            if (!success)
                throw new DatabaseOperationException("Kategori eklenemedi.");

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var success = await _categoryRepository.DeleteAsync(id);
            if (success!)
                throw new  NotFoundException("Kategori bulunamadı.");
            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryResponseList = categories.Select(p => new CategoryResponse
            {
                Id = p.Id,
                CategoryName = p.CategoryName,
                CategoryIcon = p.CategoryIcon,
            });
            return categoryResponseList;
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("Kategori bulunamadı.");
            var categoryResponse = new CategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryIcon = category.CategoryIcon
            };
            return categoryResponse;
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, CategoryRequest request, String ipAddress)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("Kategori bulunamadı.");
            existingCategory.CategoryName = request.CategoryName;
            existingCategory.CategoryIcon = request.CategoryIcon;
            existingCategory.ModifyDate = DateTime.UtcNow;
            existingCategory.ModifyIP = ipAddress;
            var success = await _categoryRepository.UpdateAsync(existingCategory);
            if (!success)
                throw new DatabaseOperationException("Kategori güncellenemedi."); ;
            return true;
        }
    }
}
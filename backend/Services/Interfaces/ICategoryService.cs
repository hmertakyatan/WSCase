using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;

namespace WorkSoftCase.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(CategoryRequest request);
        Task<bool> UpdateCategoryAsync(Guid id, CategoryRequest request, String ipAddress);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(Guid id);
    }
}
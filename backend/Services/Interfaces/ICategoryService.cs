using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Services.Results;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;

namespace WorkSoftCase.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<object>> AddCategoryAsync(CategoryRequest request);
        Task<Result<object>> UpdateCategoryAsync(Guid id, CategoryRequest request, String ipAddress);
        Task<Result<object>> DeleteCategoryAsync(Guid id);
        Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync();
        Task<Result<CategoryResponse>> GetCategoryByIdAsync(Guid id);
        Task<Result<object>> AddCategoriesRangeAsync(IEnumerable<CategoryRequest> request);
    }
}
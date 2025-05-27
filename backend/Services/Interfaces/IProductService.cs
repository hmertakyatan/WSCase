using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Services.Results;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;

namespace WorkSoftCase.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<object>> AddProductAsync(ProductRequest request);
        Task<Result<object>> UpdateProductAsync(Guid id, ProductRequest request, string modfiyerIpAddress);
        Task<Result<object>> DeleteProductAsync(Guid id);
        Task<Result<IEnumerable<ProductResponse>>> GetAllProductsAsync();
        Task<Result<ProductResponse>> GetProductByIdAsync(Guid id);
        Task<Result<IEnumerable<ProductResponse>>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> Ids);
        Task<Result<object>> AddProductsRangeAsync(IEnumerable<ProductRequest> request);
    }
}
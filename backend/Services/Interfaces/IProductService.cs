using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;

namespace WorkSoftCase.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(ProductRequest request);
        Task<bool> UpdateProductAsync(Guid id, ProductRequest request, string modfiyerIpAddress);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductResponse>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> Ids);
        Task<bool> AddProductsRangeAsync(IEnumerable<ProductRequest> request);
    }
}
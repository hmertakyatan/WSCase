using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Repository.Interfaces.IRepository;

namespace WorkSoftCase.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductResponse>> GetProductsWithCategoryAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> categoryIds);
    }
}
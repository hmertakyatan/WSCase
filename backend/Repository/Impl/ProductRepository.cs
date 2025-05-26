using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Repository.Interfaces;

namespace WorkSoftCase.Repository.Impl
{
    public class ProductRepository : DapperRepository<Product>, IProductRepository
    {


        public ProductRepository(IDbConnection connection) : base(connection)
        {
            
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> categoryIds)
        {
            var sql = "SELECT * FROM Products WHERE CategoryId IN @CategoryIds";
            return await _connection.QueryAsync<Product>(sql, new {CategoryIds = categoryIds});
        }
        
        //TODO: Create new responsedto for categoryname field.
        public async Task<IEnumerable<ProductResponse>> GetProductsWithCategoryAsync()
        {
            var sql = @"
            SELECT p.*, c.CategoryName 
            FROM Products p
            INNER JOIN Categories c ON p.CategoryId = c.Id";

            var result = await _connection.QueryAsync<Product, Category, ProductResponse>(sql,
                (product, category) => new ProductResponse
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductIcon = product.ProductIcon,
                },
                splitOn: "CategoryName"
            );

            return result;
        }
        
        
    }
}
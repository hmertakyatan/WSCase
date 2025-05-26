using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Repository.Interfaces;
using WorkSoftCase.Repository.Interfaces.IRepository;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;


        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }

        public async Task<bool> AddProductAsync(ProductRequest newProduct)
        {
            var product = new Product
            {
                ProductName = newProduct.ProductName,
                ProductIcon = newProduct.ProductIcon,
                CategoryId = newProduct.CategoryId,
            };
            var success = await _productRepository.AddAsync(product);
            if (!success)
                throw new DatabaseOperationException("Ürün eklenemedi.");

            return true;
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductRequest productRequest, string ModifyerIpAddress)
        {
            var product = await _productRepository.GetByIdAsync(id) ?? throw new NotFoundException("Ürün bulunamadı.");
            product.ProductName = productRequest.ProductName;
            product.ProductIcon = productRequest.ProductIcon;
            product.CategoryId = productRequest.CategoryId;
            product.ModifyDate = DateTime.UtcNow;
            product.ModifyIP = ModifyerIpAddress;

            var success = await _productRepository.UpdateAsync(product);
            if (!success)
                throw new DatabaseOperationException("Ürün güncellenemedi");

            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var success = await _productRepository.DeleteAsync(id);
            if (!success)
                throw new NotFoundException("Ürün bulunamadı");

            return true;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            if (!products.Any())
                throw new NotFoundException("Ürünler bulunamadı.");
            return products;
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id) ?? throw new NotFoundException("Ürün bulunamadı.");
            var productResponse = new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductIcon = product.ProductIcon,
            };


            return productResponse;
        }
        public async Task<IEnumerable<ProductResponse>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> categoryIds)
        {
            var products = await _productRepository.GetProductsByCategoryIdsAsync(categoryIds);
            if (!products.Any())
                throw new NotFoundException("Ürünler bulunamadı.");
            var result = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductIcon = p.ProductIcon,

            });

            return result;
        }
        public async Task<bool> AddProductsRangeAsync(IEnumerable<ProductRequest> productRequests)
        {
            var products = productRequests.Select(p => new Product
            {
                ProductName = p.ProductName,
                ProductIcon = p.ProductIcon,
                CategoryId = p.CategoryId
            });

            var success = await _productRepository.AddRangeAsync(products);
            if (!success)
                throw new DatabaseOperationException("Ürünler eklenemedi.");

            return true;
        }
    }
}
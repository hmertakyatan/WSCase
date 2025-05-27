using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Repository.Interfaces;
using WorkSoftCase.Repository.Interfaces.IRepository;
using WorkSoftCase.Services.Interfaces;
using WorkSoftCase.Services.Results;

namespace WorkSoftCase.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;


        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }

        public async Task<Result<object>> AddProductAsync(ProductRequest newProduct)
        {
            var product = new Product
            {
                ProductName = newProduct.ProductName,
                ProductIcon = newProduct.ProductIcon,
                CategoryId = newProduct.CategoryId,
            };
            var success = await _productRepository.AddAsync(product);
            if (!success)
                return Result<object>.Failure("Ürün eklenemedi.");

            return Result<object>.Success(message: "Ürün başarıyla eklendi.");
        }

        public async Task<Result<object>> UpdateProductAsync(Guid id, ProductRequest productRequest, string ModifyerIpAddress)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return Result<object>.Failure("Ürün bulunamadı.", statusCode: 404);
            product.ProductName = productRequest.ProductName;
            product.ProductIcon = productRequest.ProductIcon;
            product.CategoryId = productRequest.CategoryId;
            product.ModifyDate = DateTime.UtcNow;
            product.ModifyIP = ModifyerIpAddress;

            var success = await _productRepository.UpdateAsync(product);
            if (!success)
                return Result<object>.Failure(message: "Ürün güncellenemedi.");

            return  Result<object>.Success(message: "Ürün başarıyla güncellendi.");
        }

        public async Task<Result<object>> DeleteProductAsync(Guid id)
        {
            var success = await _productRepository.DeleteAsync(id);
            if (!success)
                return Result<object>.Failure(message: "Ürün silinemedi.");

            return Result<object>.Success(message: "Ürün başarıyla silindi.");
        }

        public async Task<Result<IEnumerable<ProductResponse>>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productResponses = products.Select(product => new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductIcon = product.ProductIcon,
  
            });
            return Result<IEnumerable<ProductResponse>>.Success(productResponses, "Ürünler başarıyla listelendi.");
        }

        public async Task<Result<ProductResponse>> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return Result<ProductResponse>.Failure(message: "Kullanıcı bulunamadı.");
            var productResponse = new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductIcon = product.ProductIcon,
            };


            return Result<ProductResponse>.Success(message: "Kullanıcı başarıyla getirildi.", data: productResponse);
        }
        public async Task<Result<IEnumerable<ProductResponse>>> GetProductsByCategoryIdsAsync(IEnumerable<Guid> categoryIds)
        {
            var products = await _productRepository.GetProductsByCategoryIdsAsync(categoryIds);

            var productsResponse = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductIcon = p.ProductIcon,

            });

            return Result<IEnumerable<ProductResponse>>.Success(message: "Kullanıcı başarıyla getirildi.", data: productsResponse);
        }
        public async Task<Result<object>> AddProductsRangeAsync(IEnumerable<ProductRequest> productRequests)
        {
            var products = productRequests.Select(p => new Product
            {
                ProductName = p.ProductName,
                ProductIcon = p.ProductIcon,
                CategoryId = p.CategoryId
            });

            var success = await _productRepository.AddRangeAsync(products);
            if (!success)
                return Result<object>.Failure(message: "Ürünler eklenemedi.");

            return Result<object>.Success(message: "Ürün başarıyla oluturuldu.");
        }
    }
}
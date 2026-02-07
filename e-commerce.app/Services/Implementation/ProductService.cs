using e_commerce.app.Dto;
using e_commerce.app.interfaces;
using e_commerce.app.servieses.iserviese;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.servieses.impelmentaion
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }



        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = product.Quantity,
                IsActive = product.IsActive,
                IsApproved = product.IsApproved,
                CreatedAt = product.CreatedAt,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                SellerId = product.SellerId,
                SellerName = product.Seller.UserName
            };
        }

        public async Task<IEnumerable<summaryProductDto>> GetAllAsync()
        {

            var products = await _productRepository.GetAllAsync();

            return products
                .Where(p => p.IsApproved && p.IsActive)
                .Select(p => new summaryProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.Name,
                    Quantity = p.Quantity
                });


        }
        public async Task<IEnumerable<summaryProductDto>> GetBySellerAsync(int sellerId)
        {

            var product = await _productRepository.GetBySellerAsync(sellerId);

            return product.Select(p => new summaryProductDto
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                Quantity = p.Quantity
            });

        }
        public async Task AddProductAsync(CreateProductBySellerDto dto, int sellerId)

        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                SellerId = sellerId,
                IsActive = false,
                IsApproved = false,
                CreatedAt = DateTime.UtcNow
            };
            await _productRepository.AddAsync(product);
        }
        public async Task UpdateProductAsync(int id, UpdateProductBySellerDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            if (!string.IsNullOrEmpty(dto.Name))
                product.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description))
                product.Description = dto.Description;
            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;
            if (!string.IsNullOrEmpty(dto.ImageUrl))
                product.ImageUrl = dto.ImageUrl;
            if (dto.Quantity.HasValue)
                product.Quantity = dto.Quantity.Value;
            if (dto.CategoryId.HasValue)
                product.CategoryId = dto.CategoryId.Value;
            product.IsApproved = false;
            product.IsActive = false;
            await _productRepository.UpdateAsync(product);
        }



        public async Task ApproveProductAsync(int id, ApproveProductByAdminDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.IsApproved = dto.IsApproved;
            product.IsActive = dto.IsActive;

            await _productRepository.UpdateAsync(product);
        }



        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            await _productRepository.DeleteAsync(id);
        }
    }
}

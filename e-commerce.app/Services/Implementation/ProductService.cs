using e_commerce.app.Dto.ProductDto;
using e_commerce.app.interfaces;
using e_commerce.app.servieses.iserviese;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ? ??? ??

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
                throw new NotFoundException("Product", id);

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
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.Name,
                    Quantity = p.Quantity
                });
        }

        public async Task<IEnumerable<summaryProductDto>> GetBySellerAsync(int sellerId)
        {
            var products = await _productRepository.GetBySellerAsync(sellerId);

            return products.Where(p => p.IsApproved).Select(p => new summaryProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                Quantity = p.Quantity
            });
        }

        public async Task AddProductAsync(CreateProductBySellerDto dto, int sellerId)
        {
            // ? Validate price and quantity
            if (dto.Price <= 0)
                throw new ValidationException("Price", "Price must be greater than zero.");

            if (dto.Quantity < 0)
                throw new ValidationException("Quantity", "Quantity cannot be negative.");

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

        public async Task UpdateProductAsync(int id, UpdateProductBySellerDto dto, int currentSellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            if (product.SellerId != currentSellerId)
                throw new UnauthorizedException("You are not authorized to update this product.");

            // ? Validate new values if provided
            if (dto.Price.HasValue && dto.Price.Value <= 0)
                throw new ValidationException("Price", "Price must be greater than zero.");

            if (dto.Quantity.HasValue && dto.Quantity.Value < 0)
                throw new ValidationException("Quantity", "Quantity cannot be negative.");

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

            // ??? ??????? ????? ??? Admin ???? ???? ????????
            product.IsApproved = false;
            product.IsActive = false;

            await _productRepository.UpdateAsync(product);
        }

        public async Task ApproveProductAsync(int id, ApproveProductByAdminDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            product.IsApproved = dto.IsApproved;
            product.IsActive = dto.IsActive;

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id, int currentSellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            if (product.SellerId != currentSellerId)
                throw new UnauthorizedException("You are not authorized to delete this product.");

            await _productRepository.DeleteAsync(id);
        }

        public async Task UpdateStockAsync(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            if (quantity < 0)
                throw new ValidationException("Quantity", "Stock quantity cannot be negative.");

            product.Quantity = quantity;

            await _productRepository.UpdateAsync(product);
        }

        public async Task<(IEnumerable<summaryProductDto> Products, int TotalCount)> SearchAsync(ProductSearchDto searchParams)
        {
            var result = await _productRepository.SearchAsync(searchParams);

            var mappedProducts = result.Products.Select(p => new summaryProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                Quantity = p.Quantity
            });

            return (mappedProducts, result.TotalCount);
        }

        public async Task<IEnumerable<summaryProductDto>> GetLowStockAsync(int threshold)
        {
            if (threshold < 0)
                throw new ValidationException("Threshold", "Threshold cannot be negative.");

            var products = await _productRepository.GetLowStockAsync(threshold);

            return products.Select(p => new summaryProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                Quantity = p.Quantity
            });
        }
    }
}
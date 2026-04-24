using AutoMapper;
using e_commerce.app.Dto.ShippingCartDTO;
using e_commerce.app.interfaces;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

namespace e_commerce.app.Services.Implementation
{
    public class ShoppingServiece : IShoppingServiece
    {
        private readonly IShoppingCartRepo _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingServiece(
            IShoppingCartRepo cartRepository,
            IMapper mapper,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ShoppingCartDto?> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetUserCartAsync(userId);
            return cart == null ? null : _mapper.Map<ShoppingCartDto>(cart);
        }

        public async Task<ShoppingCartDto?> UpdateCartAsync(UpdateCartDto basketDto)
        {
            // ✅ السلة فاضية
            if (basketDto.Items == null || !basketDto.Items.Any())
                throw new BusinessRuleException("Cannot update cart with no items.");

            // ✅ Validate quantities
            var invalidItem = basketDto.Items.FirstOrDefault(i => i.Quantity <= 0);
            if (invalidItem != null)
                throw new ValidationException("Quantity", "All item quantities must be greater than zero.");

            var productIds = basketDto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // ✅ تأكد إن كل المنتجات موجودة وactive
            foreach (var item in basketDto.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null || !product.IsApproved || !product.IsActive)
                    throw new NotFoundException("Product", item.ProductId);
            }

            var cartEntity = _mapper.Map<ShopingCart>(basketDto);
            if (cartEntity == null)
                throw new BusinessRuleException("Failed to process cart update.");

            foreach (var item in cartEntity.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                    item.PriceAtTime = product.Price;
            }

            var updatedCart = await _cartRepository.UpdateCartAsync(cartEntity);
            return _mapper.Map<ShoppingCartDto>(updatedCart);
        }

        public async Task AddItemsToCartAsync(int userId, int productId, int quantity)
        {
            // ✅ Validate quantity
            if (quantity <= 0)
                throw new ValidationException("Quantity", "Quantity must be greater than zero.");

            // ✅ السلة مش موجودة
            var cart = await _cartRepository.GetUserCartAsync(userId);
            if (cart == null)
                throw new NotFoundException("Cart", userId);

            // ✅ المنتج مش موجود أو مش approved
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsApproved || !product.IsActive)
                throw new NotFoundException("Product", productId);

            // ✅ المخزون مش كفاية
            if (product.Quantity < quantity)
                throw new BusinessRuleException(
                    $"Only {product.Quantity} units of '{product.Name}' are available.");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // ✅ تأكد إن الـ total quantity مش هتعدي المخزون
                if (existingItem.Quantity + quantity > product.Quantity)
                    throw new BusinessRuleException(
                        $"Cannot add {quantity} more units. Only {product.Quantity - existingItem.Quantity} units remaining.");

                existingItem.Quantity += quantity;
                await _cartRepository.UpdateCartAsync(cart);
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = cart.Id
                };
                await _cartRepository.AddItemToCartAsync(cart.Id, newItem);
            }
        }

        public async Task<bool> DeleteCartItemsAsync(int userId)
        {
            return await _cartRepository.DeleteCartItemsAsync(userId);
        }
    }
}
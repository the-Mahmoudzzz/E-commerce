using AutoMapper;
using e_commerce.app.Dto.ShippingCartDTO;
using e_commerce.app.interfaces;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class ShoppingServiece : IShoppingServiece
    {
        private readonly IShoppingCartRepo _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingServiece(IShoppingCartRepo cartRepository, IMapper mapper, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;

            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ShoppingCartDto?> GetCartAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            return cart == null ? null : _mapper.Map<ShoppingCartDto>(cart);
        }

        public async Task<ShoppingCartDto?> UpdateCartAsync(ShoppingCartDto basketDto)
        {
            var productIds = basketDto.Items.Select(i => i.ProductId).Distinct().ToList();

            // 2. نجيب كل المنتجات دي من الداتا بيز في Query واحدة بس!
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // 3. نلّف على السلة ونحدث الأسعار من الميموري (بدون ما نكلم الداتا بيز تاني)
            foreach (var item in basketDto.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    item.Price = product.Price;
                    item.ProductName = product.Name;
                }
            }

            // 4. نحول من DTO لـ Entity
            var cartEntity = _mapper.Map<ShopingCart>(basketDto);

            // 5. نحفظ في الداتا بيز عن طريق الـ Repo بتاع السلة
            var updatedCart = await _cartRepository.UpdateCartAsync(cartEntity);

            return _mapper.Map<ShoppingCartDto>(updatedCart);
        }
        public async Task AddItemsToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetCartAsync(userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = cart.Id
                });
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            return await _cartRepository.DeleteCartAsync(cartId);
        }
    }
}

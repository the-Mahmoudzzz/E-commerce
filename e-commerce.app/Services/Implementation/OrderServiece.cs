using AutoMapper;
using e_commerce.app.Dto.OrderDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class OrderServiece : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IShoppingCartRepo _cartRepo;
        private readonly IShippingZoneRepo _shippingZoneRepo;
        private readonly IDiscountRepo _discountRepo;
        private readonly IMapper _mapper;

        public OrderServiece(
            IOrderRepo orderRepo,
            IShoppingCartRepo cartRepo,
            IShippingZoneRepo shippingZoneRepo,
            IDiscountRepo discountRepo,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _shippingZoneRepo = shippingZoneRepo;
            _discountRepo = discountRepo;
            _mapper = mapper;
        }

        public async Task CreateOrder(OrderCreateDto orderDto)
        {
            var cart = await _cartRepo.GetCartAsync(orderDto.CartId);
            if (cart == null || !cart.Items.Any())
                throw new Exception("Not Found");

            decimal totalPrice = cart.Items.Sum(item => item.PriceAtTime * item.Quantity);

            var zone = await _shippingZoneRepo.GetByIdAsync(orderDto.ShippingZoneId);
            decimal shippingCost = zone != null ? zone.ShipingCost : 0;

            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(orderDto.DiscountCode))
            {
                var discount = await _discountRepo.GetActiveDiscountByCodeAsync(orderDto.DiscountCode);
                if (discount != null && totalPrice >= discount.MinOrderAmount)
                {
                    discountAmount = discount.Value;
                }
            }

            decimal finalAmount = (totalPrice + shippingCost) - discountAmount;

            var orderDetails = cart.Items.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtTime = item.PriceAtTime,
                Status = OrderStatus.Pending
            }).ToList();

            var order = new Order
            {
                CustomerId = cart.CustmerId, 
                TotalPrice = totalPrice,
                ShippingCost = shippingCost,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderDetails = orderDetails
            };

            await _orderRepo.CreateOrderAsync(order);
            await _cartRepo.DeleteCartAsync(cart.Id);
        }

        public async Task<IReadOnlyList<OrderDTO>> GetOrderByCustomerId(int customeid)
        {
            var orders = await _orderRepo.GetOrderByUser(customeid);
            return _mapper.Map<IReadOnlyList<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderById(int orderid)
        {
            var order = await _orderRepo.GetOrderById(orderid);
            if (order == null) return null;
            return _mapper.Map<OrderDTO>(order);
        }
    }
}
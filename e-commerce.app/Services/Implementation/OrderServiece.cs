using AutoMapper;
using e_commerce.app.Dto.OrderDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IShoppingServiece _shoppingServiece;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShippingService _shippingService;
        private readonly IDiscountService _discountService;



        public OrderServiece(
            IOrderRepo orderRepo,
            IShoppingCartRepo cartRepo,
            IMapper mapper,
            IShoppingServiece shoppingServiece,
                IHttpContextAccessor httpContextAccessor
,
                IShippingService shippingService,
                IDiscountService discountService)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _mapper = mapper;
            _shoppingServiece = shoppingServiece;
            _httpContextAccessor = httpContextAccessor;
            _shippingService = shippingService;
            this._discountService = discountService;
        }

        public async Task CreateOrder(OrderCreateDto orderDto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("User not authenticated");


            int userId = int.Parse(userIdClaim);
           
            var cart = await _shoppingServiece.GetCartAsync(orderDto.CartId);
            if (cart == null || !cart.Items.Any())
                throw new Exception("السلة فاضية أو غير موجودة");
            

            decimal totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);
            Console.WriteLine(totalPrice);

            var zone = await _shippingService.GetZoneAsync(orderDto.ShippingZoneId);
            decimal shippingCost = zone != null ? zone.ShippingCost : 0;

      
            decimal discountAmount = 0;

            if (!string.IsNullOrEmpty(orderDto.DiscountCode))
            {
                var discount = await _discountService
                    .ApplyDiscountAsync(orderDto.DiscountCode, totalPrice);

                if (discount.DiscountType == "Percentage")
                    discountAmount = totalPrice * (discount.Value / 100);
                else
                    discountAmount = discount.Value;
            }

          
            decimal finalAmount = (totalPrice + shippingCost) - discountAmount;

            var orderDetails = cart.Items.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtTime = item.Price,
                Status = OrderStatus.Pending
            }).ToList();

          
            var order = new Order
            {
               
                CustomerId = userId,
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


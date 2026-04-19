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
        private readonly INotificationService _notificationService;



        public OrderServiece(
            IOrderRepo orderRepo,
            IShoppingCartRepo cartRepo,
            IMapper mapper,
            IShoppingServiece shoppingServiece,
                IHttpContextAccessor httpContextAccessor
,
                IShippingService shippingService,
                IDiscountService discountService,
                INotificationService notificationService)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _mapper = mapper;
            _shoppingServiece = shoppingServiece;
            _httpContextAccessor = httpContextAccessor;
            _shippingService = shippingService;
            this._discountService = discountService;
            _notificationService = notificationService;
        }

        public async Task CreateOrder(OrderCreateDto orderDto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("User not authenticated");


            int userId = int.Parse(userIdClaim);
            // 1. نجيب السلة بالمنتجات بتاعتها
            // (بفترض إنك عامل ميثود GetCartAsync في الـ IShoppingCartRepo)
            var cart = await _shoppingServiece.GetCartAsync(orderDto.CartId);
            if (cart == null || !cart.Items.Any())
                throw new Exception("السلة فاضية أو غير موجودة");
            

            // 2. نحسب الإجمالي بتاع المنتجات
            decimal totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);
            Console.WriteLine(totalPrice);

            // 3. نحسب تكلفة الشحن من جدول الـ ShippingZones
            var zone = await _shippingService.GetZoneAsync(orderDto.ShippingZoneId);
            decimal shippingCost = zone != null ? zone.ShippingCost : 0;

            // 4. نحسب الخصم لو العميل باعت كود خصم
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

            // 5. الحسبة النهائية
            decimal finalAmount = (totalPrice + shippingCost) - discountAmount;

            // 6. نجهز المنتجات عشان تتنقل لجدول OrderDetails
            var orderDetails = cart.Items.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtTime = item.Price,
                Status = OrderStatus.Pending // الحالة الافتراضية
            }).ToList();

            // 7. نكريت الـ Order Entity الأساسية
            var order = new Order
            {
                // ملاحظة: تأكد إن الـ OrderCreateDto فيه CustomerId، أو بتجيبه من التوكن
                CustomerId = userId,
                TotalPrice = totalPrice,
                ShippingCost = shippingCost,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderDetails = orderDetails
            };

            

            // 8. نحفظ الأوردر في الداتا بيز (الريبو بتاعك بيعمل SaveChanges جواه فمش محتاجين نعملها هنا)
            await _orderRepo.CreateOrderAsync(order);
            await _notificationService.AddNotifiAsync(new Dto.NotificationDto.CreateNotificationDto
            {
                Message = $"Your oreder  {order.Id} is Confiremed",
                UserId = userId,
                Title = "Order Is Confiermed"
            }
            );
            // 9. نمسح السلة عشان العميل ميطلبهاش تاني بالغلط
            await _cartRepo.DeleteCartAsync(cart.Id);
        }

        public async Task<IReadOnlyList<OrderDTO>> GetOrderByCustomerId(int customeid)
        {
            // بنجيب الداتا من الريبو
            var orders = await _orderRepo.GetOrderByUser(customeid);

            // بنحولها لـ DTO عشان نعرضها للـ Frontend
            return _mapper.Map<IReadOnlyList<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderById(int orderid)
        {
            // بنجيب الأوردر بالـ Includes اللي إنت كاتبها في الريبو
            var order = await _orderRepo.GetOrderById(orderid);

            // لو مش موجود ممكن نرجع null أو نرمي Exception حسب اللوجيك بتاعك
            if (order == null) return null;

            // بنحوله لـ DTO
            return _mapper.Map<OrderDTO>(order);
        }
    }
}


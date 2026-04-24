using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Dto.OrderDto;
using e_commerce.app.interfaces;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using e_commerce.core.Exceptions;          // ← ضيف ده
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
        private readonly IProductRepository _productRepository;

        public OrderServiece(
            IOrderRepo orderRepo,
            IShoppingCartRepo cartRepo,
            IMapper mapper,
            IShoppingServiece shoppingServiece,
            IHttpContextAccessor httpContextAccessor,
            IShippingService shippingService,
            IDiscountService discountService,
            INotificationService notificationService,
            IProductRepository productRepository)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _mapper = mapper;
            _shoppingServiece = shoppingServiece;
            _httpContextAccessor = httpContextAccessor;
            _shippingService = shippingService;
            _discountService = discountService;
            _notificationService = notificationService;
            _productRepository = productRepository;
        }

        // ✅ Helper مشترك عشان نجيب الـ userId من الـ Token في أي مكان
        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new AuthenticationException("User is not authenticated.");

            return int.Parse(userIdClaim);
        }

        public async Task CreateOrder(OrderCreateDto orderDto)
        {
            int userId = GetCurrentUserId();

            // ✅ السلة فاضية
            var cart = await _shoppingServiece.GetCartAsync(userId);
            if (cart == null || !cart.Items.Any())
                throw new BusinessRuleException("Your cart is empty. Add items before placing an order.");

            // ✅ Stock check لكل منتج
            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new NotFoundException("Product", item.ProductId);

                if (product.Quantity < item.Quantity)
                    throw new BusinessRuleException(
                        $"'{product.Name}' has only {product.Quantity} units available, but you requested {item.Quantity}.");
            }

            // حساب الإجمالي
            decimal totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);

            // ✅ Shipping zone مش موجودة
            var zone = await _shippingService.GetZoneAsync(orderDto.ShippingZoneId);
            if (zone == null)
                throw new NotFoundException("ShippingZone", orderDto.ShippingZoneId);

            decimal shippingCost = zone.ShippingCost;

            // تطبيق الكوبون لو موجود
            decimal discountAmount = 0;
            if (!string.IsNullOrWhiteSpace(orderDto.DiscountCode))
            {
                // ✅ ApplyDiscountAsync هتعمل throw لو الكوبون غلط أو منتهي
                var discount = await _discountService.ApplyDiscountAsync(orderDto.DiscountCode, totalPrice);

                discountAmount = discount.DiscountType == DiscountType.Percentage
                    ? totalPrice * (discount.Value / 100)
                    : discount.Value;
            }

            decimal finalAmount = totalPrice - discountAmount + shippingCost;

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

            await _notificationService.AddNotifiAsync(new CreateNotificationDto
            {
                UserId = userId,
                Title = "Order Confirmed",
                Message = $"Your order #{order.Id} has been placed successfully."
            });

            await _cartRepo.DeleteCartItemsAsync(userId);
        }

        public async Task CancelOrder(int customerId, int orderId)
        {
            var order = await _orderRepo.GetOrderById(orderId);

            // ✅ Order مش موجود
            if (order == null)
                throw new NotFoundException("Order", orderId);

            // ✅ الأوردر مش بتاع الكاستمر ده
            if (order.CustomerId != customerId)
                throw new UnauthorizedException("You are not authorized to cancel this order.");

            // ✅ مش في حالة Pending
            if (order.Status != OrderStatus.Pending)
                throw new BusinessRuleException("Only pending orders can be canceled.");

            order.Status = OrderStatus.Canceled;

            foreach (var item in order.OrderDetails)
            {
                item.Status = OrderStatus.Canceled;

                // رجّع المخزون
                if (item.Product != null)
                    item.Product.Quantity += item.Quantity;
            }

            // إبعت نوتيفيكيشن للسيلر/ز
            var sellerIds = order.OrderDetails
                .Select(i => i.Product.SellerId)
                .Distinct();

            foreach (var sellerId in sellerIds)
            {
                await _notificationService.AddNotifiAsync(new CreateNotificationDto
                {
                    UserId = sellerId,
                    Title = "Order Canceled",
                    Message = $"Order #{order.Id} has been canceled by the customer."
                });
            }

            // إبعت نوتيفيكيشن للكاستمر
            await _notificationService.AddNotifiAsync(new CreateNotificationDto
            {
                UserId = customerId,
                Title = "Order Canceled",
                Message = $"Your order #{order.Id} has been canceled successfully."
            });

            await _orderRepo.UpdateOrder(order);
        }

        public async Task<IReadOnlyList<OrderDTO>> GetOrderByCustomerId(int customerId)
        {
            var orders = await _orderRepo.GetOrderByUser(customerId);

            // ✅ مش error إن مفيش أوردرات — بس نرجع list فاضية
            if (orders == null || !orders.Any())
                return new List<OrderDTO>();

            return _mapper.Map<IReadOnlyList<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var order = await _orderRepo.GetOrderById(orderId);

            // ✅ بدل ما نرجع null
            if (order == null)
                throw new NotFoundException("Order", orderId);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetIncomingOrder()
        {
            int userId = GetCurrentUserId();

            var orders = await _orderRepo.GetIncomingOrder(userId);

            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
}
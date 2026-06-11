using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Dto.PayMentDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using e_commerce.core.Exceptions;          // ← ضيف ده
using Microsoft.Extensions.Configuration;
using System.Transactions;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repo;
    private readonly IPaymobService _paymob;
    private readonly IOrderRepo _orderRepo;
    private readonly IConfiguration _config;
    private readonly INotificationService _notificationService;

    public PaymentService(
        IPaymentRepository repo,
        IPaymobService paymob,
        IOrderRepo orderRepo,
        IConfiguration config,
        INotificationService notificationService)
    {
        _repo = repo;
        _orderRepo = orderRepo;
        _paymob = paymob;
        _config = config;
        _notificationService = notificationService;
    }

    public async Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto)
    {
        // ✅ الأوردر مش موجود
        var order = await _orderRepo.GetOrderById(dto.OrderId);
        if (order == null)
            throw new NotFoundException("Order", dto.OrderId);

        // ✅ الأوردر مش في حالة Pending — يعني اتدفع قبل كده أو اتكنسل
        if (order.Status != OrderStatus.Pending)
            throw new BusinessRuleException(
                $"Cannot create payment for an order with status '{order.Status}'.");

        // ✅ في payment موجود بالفعل للأوردر ده
        var existingPayment = await _repo.GetByOrderIdAsync(order.Id);
        if (existingPayment != null && existingPayment.Status == PaymentStatus.Approved)
            throw new ConflictException($"Order #{order.Id} has already been paid.");

        // ✅ الـ IframeId مش موجود في الـ config
        var iframeId = _config["Paymob:IframeId"];
        if (string.IsNullOrEmpty(iframeId))
            throw new BusinessRuleException("Paymob Iframe ID is not configured.");

        var payment = new Payment
        {
            OrderId = order.Id,
            PaymentMethod = dto.PaymentMethod,
            Status = PaymentStatus.Pending,
            Amount = order.FinalAmount
        };

        await _repo.AddAsync(payment);

        // ✅ الـ PaymobService هيعمل throw لو فيه مشكلة — الـ Middleware هيمسكها
        var token = await _paymob.GetAuthToken();
        var paymobOrderId = await _paymob.CreateOrder(token, payment.Amount);
        var paymentKey = await _paymob.GetPaymentKey(token, paymobOrderId, payment.Amount);

        var iframeUrl =
            $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";

        payment.TransactionReference = paymobOrderId.ToString();
        await _repo.UpdateAsync(payment);

        return new PaymentResponseDto
        {
            PaymentId = payment.Id,
            PaymentUrl = iframeUrl
        };
    }

    public async Task HandleCallbackAsync(PaymobCallbackDto data)
    {
        // ✅ Callback — لو الـ payment مش موجود نخرج بهدوء (مش error)
        // ممكن يكون webhook قديم أو duplicate
        var payment = await _repo.GetByTransactionRef(data.Obj.Order.Id.ToString());
        if (payment == null) return;

        // ✅ تجاهل لو الـ payment اتعالج قبل كده (idempotency)
        if (payment.Status != PaymentStatus.Pending) return;
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            if (data.Obj.Success)
            {
                payment.Status = PaymentStatus.Approved;
                payment.PaidAt = DateTime.UtcNow;

                var order = await _orderRepo.GetOrderById(payment.OrderId);

                // ✅ لو الأوردر مش موجود نسجل الـ error بس ومنكرهوش
                if (order != null)
                {
                    order.Status = OrderStatus.Processing;
                    await _orderRepo.UpdateOrder(order);

                    await _notificationService.AddNotifiAsync(new CreateNotificationDto
                    {
                        UserId = order.CustomerId,
                        Title = "Payment Confirmed",
                        Message = $"Your payment for order #{order.Id} was successful."
                    });
                }
            }
            else
            {
                payment.Status = PaymentStatus.Refused;

                var order = await _orderRepo.GetOrderById(payment.OrderId);
                if (order != null)
                {
                    await _notificationService.AddNotifiAsync(new CreateNotificationDto
                    {
                        UserId = order.CustomerId,
                        Title = "Payment Failed",
                        Message = $"Your payment for order #{order.Id} was declined. Please try again."
                    });
                }
            }

            await _repo.UpdateAsync(payment);
            scope.Complete();
                
        }
    }
}
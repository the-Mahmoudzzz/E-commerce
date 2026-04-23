using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Dto.PayMentDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.Extensions.Configuration;
using System;

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
        var order = await _orderRepo.GetOrderById(dto.OrderId);

        if (order == null)
            throw new Exception("Order not found");
        if (order.Status != OrderStatus.Pending)
            throw new Exception($"Order is already {order.Status} ");

        var payment = new Payment
        {
            OrderId = order.Id,
            PaymentMethod = dto.PaymentMethod,
            Status = PaymentStatus.Pending,
            Amount = order.FinalAmount
        };

        await _repo.AddAsync(payment);

        var token = await _paymob.GetAuthToken();
        var paymobOrderId = await _paymob.CreateOrder(token, payment.Amount);
        var paymentKey = await _paymob.GetPaymentKey(token, paymobOrderId, payment.Amount);

        var iframeId = _config["Paymob:IframeId"];

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
        bool success = data.Obj.Success;
        int paymobOrderId = data.Obj.Order.Id;

        var payment = await _repo.GetByTransactionRef(paymobOrderId.ToString());

        if (payment == null) return;

        if (success)
        {
            payment.Status = PaymentStatus.Approved;
            payment.PaidAt = DateTime.UtcNow;

            var order = await _orderRepo.GetOrderById(payment.OrderId);
            order.Status = OrderStatus.Processing;
            await _orderRepo.UpdateOrder(order);
            await _notificationService.AddNotifiAsync(new e_commerce.app.Dto.NotificationDto.CreateNotificationDto
            {
                Message = $"Payment Sacssefully ",

                Title = "Confirm Payment",
                UserId = order.CustomerId

            });
        }
        else
        {
            payment.Status = PaymentStatus.Refused;
            
        }

        await _repo.UpdateAsync(payment);
    }
}
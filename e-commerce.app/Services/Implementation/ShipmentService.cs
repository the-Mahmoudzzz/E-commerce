using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Dto.ShipmentDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using e_commerce.core.Exceptions;          // ← ضيف ده

namespace e_commerce.app.Services.Implementation
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepo _repo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ShipmentService(
            IShipmentRepo repo,
            IMapper mapper,
            INotificationService notificationService)
        {
            _repo = repo;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ShipmentDto> GetByIdAsync(int id)
        {
            var shipment = await _repo.GetByIdAsync(id);
            if (shipment == null)
                throw new NotFoundException("Shipment", id);

            return _mapper.Map<ShipmentDto>(shipment);
        }

        public async Task CreateAsync(ShipmentCreateDto dto)
        {
             
            var existing = await _repo.GetByOrderIdAsync(dto.OrderId);
            if (existing != null)
                throw new ConflictException($"A shipment already exists for order #{dto.OrderId}.");

            var shipment = _mapper.Map<Shipment>(dto);
            shipment.Status = ShipmentStatus.Preparing;
            shipment.ShippedDate = DateTime.UtcNow;

            await _repo.AddAsync(shipment);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, ShipmentUpdateDto dto)
        {
            var shipment = await _repo.GetByIdAsync(id);
            if (shipment == null)
                throw new NotFoundException("Shipment", id);

            // ✅ منعاش نرجع لحالة سابقة
            if (dto.Status < shipment.Status)
                throw new BusinessRuleException(
                    $"Cannot change shipment status from '{shipment.Status}' back to '{dto.Status}'.");

            // ✅ لو هي delivered أصلاً
            if (shipment.Status == ShipmentStatus.Delivered)
                throw new BusinessRuleException("This shipment has already been delivered.");

            shipment.Status = dto.Status;

            if (dto.Status == ShipmentStatus.Shipped)
            {
                // ✅ Tracking number مطلوب لما يتشحن
                if (string.IsNullOrWhiteSpace(dto.TrackingNumber.ToString()))
                    throw new ValidationException("TrackingNumber", "Tracking number is required when marking shipment as shipped.");

                shipment.TrackingNumber = int.Parse(dto.TrackingNumber.ToString());

                await _notificationService.AddNotifiAsync(new CreateNotificationDto
                {
                    UserId = shipment.order.CustomerId,
                    Title = "Your order is on the way!",
                    Message = $"Your order has been shipped. Tracking number: {dto.TrackingNumber}"
                });
            }

            if (dto.Status == ShipmentStatus.Delivered)
            {
                shipment.DelevierdDate = DateTime.UtcNow;

                await _notificationService.AddNotifiAsync(new CreateNotificationDto
                {
                    UserId = shipment.order.CustomerId,
                    Title = "Order Delivered",
                    Message = "Your order has been delivered. We'd love to hear your feedback!"
                });
            }

            _repo.Update(shipment);
            await _repo.SaveChangesAsync();
        }
    }
}
using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Dto.ShipmentDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepo _repo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ShipmentService(IShipmentRepo repo, IMapper mapper, INotificationService notificationService)
        {
            _repo = repo;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ShipmentDto> GetByIdAsync(int id)
        {
            var shipment = await _repo.GetByIdAsync(id);

            if (shipment == null)
                throw new Exception("Shipment not found");

            
            return _mapper.Map<ShipmentDto>(shipment);
        }

        public async Task CreateAsync(ShipmentCreateDto dto)
        {
            
            var shipment = _mapper.Map<Shipment>(dto);
            shipment.Status = ShipmentStatus.Preparing;
            shipment.ShippedDate = DateTime.Now;

            await _repo.AddAsync(shipment);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, ShipmentUpdateDto dto)
        {
            var shipment = await _repo.GetByIdAsync(id);

            if (shipment == null)
                throw new Exception("Shipment not found");

            shipment.Status = dto.Status;
            if (dto.Status == ShipmentStatus.Shipped)
                await _notificationService.AddNotifiAsync(new CreateNotificationDto
                {
                    UserId = shipment.order.CustomerId,
                    Title = "your order is beeing ship",
                    Message = $"trakig number : {dto.TrackingNumber}"
                });


            if (dto.Status == ShipmentStatus.Delivered)
            {
                shipment.DelevierdDate = DateTime.Now;
                await _notificationService.AddNotifiAsync(new CreateNotificationDto
                {
                    UserId = shipment.order.CustomerId,
                    Title = "are the order have deliverd ? feedback us",
                    Message = $"{dto.TrackingNumber}"
                });
            }

            _repo.Update(shipment);
            await _repo.SaveChangesAsync();
        }
    }
}

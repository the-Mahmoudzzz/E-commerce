using AutoMapper;
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

        public ShipmentService(IShipmentRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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

            
            if (dto.Status == ShipmentStatus.Delivered)
            {
                shipment.DelevierdDate = DateTime.Now;
            }

            _repo.Update(shipment);
            await _repo.SaveChangesAsync();
        }
    }
}

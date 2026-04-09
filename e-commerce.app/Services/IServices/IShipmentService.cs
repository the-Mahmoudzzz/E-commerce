using e_commerce.app.Dto.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IShipmentService
    {
        Task<ShipmentDto> GetByIdAsync(int id);
        Task CreateAsync(ShipmentCreateDto dto);
        Task UpdateStatusAsync(int id, ShipmentUpdateDto dto);
    }
}

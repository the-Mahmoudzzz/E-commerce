using e_commerce.app.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IShippingService
    {
        Task<ShippingZoneDto> GetZoneAsync(int id);
        Task<IReadOnlyList<ShippingZoneDto>> GetAllZonesAsync();
        Task AddZoneAsync(ShippingZoneDto zoneDto);
        Task UpdateZoneAsync(ShippingZoneDto zoneDto);
        Task DeleteZoneAsync(int id);
    }
}

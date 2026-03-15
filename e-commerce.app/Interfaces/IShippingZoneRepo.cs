using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IShippingZoneRepo
    {
        Task<ShippingZone> GetByIdAsync(int id);

        Task<IReadOnlyList<ShippingZone>> GetAllAsync();

        Task AddAsync(ShippingZone zone);

        Task UpdateAsync(ShippingZone zone);

        Task DeleteAsync(int id);
    }
}

using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IShipmentRepo
    {
        Task<Shipment> GetByIdAsync(int id);
        Task AddAsync(Shipment shipment);
        void Update(Shipment shipment);
        Task SaveChangesAsync();
        Task<Shipment> GetByOrderIdAsync(int orderId);
    }
}

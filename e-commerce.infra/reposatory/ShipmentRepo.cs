using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class ShipmentRepo : IShipmentRepo
    {
        private readonly AppDbContext _context;

        public ShipmentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await _context.shipments
                .Include(s => s.order)
                .Include(s => s.ShippingZone)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Shipment> GetByOrderIdAsync(int orderId)
        {
            return await _context.shipments
                                 .FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task AddAsync(Shipment shipment)
        {
            await _context.shipments.AddAsync(shipment);
        }

        public void Update(Shipment shipment)
        {
            _context.shipments.Update(shipment);
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

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
    public class ShippingZoneRepo :IShippingZoneRepo
    {
        private readonly AppDbContext _context;

        public ShippingZoneRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShippingZone> GetByIdAsync(int id)
        {
            return await _context.shippingZones
                .FirstOrDefaultAsync(z => z.Id == id && z.IsActive);
        }

        public async Task<IReadOnlyList<ShippingZone>> GetAllAsync()
        {
            return await _context.shippingZones
                .Where(z => z.IsActive)
                .ToListAsync();
        }

        public async Task AddAsync(ShippingZone zone)
        {
            await _context.shippingZones.AddAsync(zone);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShippingZone zone)
        {
            _context.shippingZones.Update(zone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var zone = await _context.shippingZones.FindAsync(id);
            if (zone != null)
            {
                zone.IsActive = false; 
                await _context.SaveChangesAsync();
            }
        }
    }
}


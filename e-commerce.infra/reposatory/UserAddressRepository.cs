using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.infra.reposatory
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly AppDbContext _context;

        public UserAddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAddresse>> GetByUserIdAsync(int userId)
            => await _context.userAddresses
                .Where(a => a.CustomerId == userId)
                .ToListAsync();

        public async Task<UserAddresse> GetByIdAsync(int addressId)
            => await _context.userAddresses.FindAsync(addressId);

        public async Task<UserAddresse> AddAsync(UserAddresse address)
        {
            await _context.userAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<UserAddresse> UpdateAsync(UserAddresse address)
        {
            _context.userAddresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(int addressId)
        {
            var address = await GetByIdAsync(addressId);
            if (address == null) return false;

            _context.userAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ResetDefaultAsync(int userId)
        {
            var addresses = await _context.userAddresses
                .Where(a => a.CustomerId == userId && a.IsDefault)
                .ToListAsync();

            addresses.ForEach(a => a.IsDefault = false);
            await _context.SaveChangesAsync();
        }
    }
}

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
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetByUserIdAsync(int userId)
            => await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

        public async Task<Wishlist> GetByUserAndProductAsync(int userId, int productId)
            => await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

        public async Task<Wishlist> AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> RemoveAsync(int userId, int productId)
        {
            var item = await GetByUserAndProductAsync(userId, productId);
            if (item == null) return false;

            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int userId, int productId)
            => await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);
    }
}

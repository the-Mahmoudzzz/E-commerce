using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly AppDbContext _context; 

        public ShoppingCartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShopingCart?> GetCartAsync(int cartId)
        {
            return await _context.shopingCarts
                .Include(c => c.Items).ThenInclude(p=>p.Product) 
                .FirstOrDefaultAsync(c =>c.Id==cartId );
        }

        public async Task<ShopingCart?> UpdateCartAsync(ShopingCart cart)
        {
            var existingCart = await _context.shopingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (existingCart == null)
            {
                _context.shopingCarts.Add(cart);
            }
            else
            {
               _context.shoppingCartItems.RemoveRange(existingCart.Items);
                existingCart.Items = cart.Items;
            }

            await _context.SaveChangesAsync();
            return await GetCartAsync(cart.Id); 
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            var cart = await _context.shopingCarts.FindAsync(cartId);
            if (cart == null) return false;

            _context.shopingCarts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

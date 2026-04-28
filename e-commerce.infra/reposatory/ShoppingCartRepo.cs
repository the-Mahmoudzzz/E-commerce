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

        public async Task<ShopingCart?> GetUserCartAsync(int userId)
        {
            return await _context.shopingCarts
                .Include(s => s.Custmoer)
                .Include(c => c.Items)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.CustmerId == userId);
        }
        public async Task AddCatToUserAsync(int customerid)
        {
            var existingCart = await _context.shopingCarts
        .FirstOrDefaultAsync(c => c.CustmerId == customerid);

            if (existingCart != null)
                throw new Exception("the user has already cart"); 

            var cart = new ShopingCart
            {
                 CustmerId = customerid,
            };

            await _context.shopingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();

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
            return await _context.shopingCarts
        .Include(s => s.Custmoer)
        .Include(c => c.Items)
            .ThenInclude(p => p.Product)
        .FirstOrDefaultAsync(c => c.Id == cart.Id);
        }
        public async Task AddItemToCartAsync(int cartId, ShoppingCartItem item)
        {
            var cart = await _context.shopingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new Exception("Cart not found");

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.shoppingCartItems.Update(existingItem);
            }
            else
            {
                item.ShoppingCartId = cartId;
                await _context.shoppingCartItems.AddAsync(item);
            }
          

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCartItemsAsync(int userid)
        {
            var cartExists =await GetUserCartAsync(userid);

            if (cartExists==null)
                return false;
            var cartitem = await _context.shoppingCartItems
                .Where(s=>s.ShoppingCartId==cartExists.Id).ToListAsync();
            if (cartitem.Any()) {
                _context.shoppingCartItems.RemoveRange(cartitem);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteItemInCartAsync(int userid, int productid)
        {
            var cart = await GetUserCartAsync(userid);

            if (cart == null)
                return false;

            var item = await _context.shoppingCartItems
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productid &&
                    p.ShoppingCartId == cart.Id);
            

            if (item == null)
                return false;

            _context.shoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

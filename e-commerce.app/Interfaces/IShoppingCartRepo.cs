using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IShoppingCartRepo
    {
        Task<ShopingCart?> GetCartAsync(int cartId);
        Task AddCatToUserAsync(int customerid);
        Task AddItemToCartAsync(int cartId, ShoppingCartItem item);



        Task<ShopingCart?> UpdateCartAsync(ShopingCart cart);

        
        Task<bool> DeleteCartAsync(int cartId);
    }
}

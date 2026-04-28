using e_commerce.app.Dto.ShippingCartDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IShoppingServiece
    {
        Task<ShoppingCartDto?> GetCartAsync(int cartId);
        Task<ShoppingCartDto?> UpdateCartAsync(UpdateCartDto basketDto);
        Task AddItemsToCartAsync(int userId, int productId, int quantity);
        Task<bool> DeleteCartItemsAsync(int userid);
        Task<bool> DeleteItemInCartAsync(int userid,int productid);
    }
}

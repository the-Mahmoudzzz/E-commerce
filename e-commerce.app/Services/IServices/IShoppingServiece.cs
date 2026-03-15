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
        Task<ShoppingCartDto?> UpdateCartAsync(ShoppingCartDto basketDto);
        Task<bool> DeleteCartAsync(int cartId);
    }
}

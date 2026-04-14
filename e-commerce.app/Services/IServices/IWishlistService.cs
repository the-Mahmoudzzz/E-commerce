using e_commerce.app.Dto.WishlistDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDto>> GetUserWishlistAsync(int userId);
        Task<WishlistDto> AddToWishlistAsync(AddToWishlistDto dto);
        Task<bool> RemoveFromWishlistAsync(int userId, int productId);
        Task<bool> IsProductInWishlistAsync(int userId, int productId);
    }
}

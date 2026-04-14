using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetByUserIdAsync(int userId);
        Task<Wishlist> GetByUserAndProductAsync(int userId, int productId);
        Task<Wishlist> AddAsync(Wishlist wishlist);
        Task<bool> RemoveAsync(int userId, int productId);
        Task<bool> ExistsAsync(int userId, int productId);
    }
}

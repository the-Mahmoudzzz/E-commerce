using e_commerce.app.Dto.ProductReviewDTO;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IReviewProductService
    {
        Task AddAsync(AddProductReviewDTO productReviewDTO);
        Task<IEnumerable<ProductReviewDTO>> GetByProductIdAsync(
       int productId,
       bool onlyApproved = true
   );
        Task<ProductReviewDTO?> GetByIdAsync(int id);
        Task UpdateAsync(int reviewId, UpdateProductReviewDTO productReview);
        Task DeleteAsync(int id);
    }
}

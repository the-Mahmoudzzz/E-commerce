using e_commerce.app.Dto.ProductReviewDTO;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IReviewProductRepo
    {
        Task AddAsync(ProductReview productReview);
        Task<IEnumerable<ProductReview>> GetByProductIdAsync(
       int productId,
       bool onlyApproved = true
   );
        Task<ProductReview> GetById(int id);
        Task<ProductReview> UpdateAsync(ProductReview productReview);
        Task DeleteAsync(int id);
    }
}

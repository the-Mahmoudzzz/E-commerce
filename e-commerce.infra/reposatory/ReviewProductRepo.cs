using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class ReviewProductRepo : IReviewProductRepo
    {
        private readonly AppDbContext _appContext;
        public ReviewProductRepo(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public async Task AddAsync(ProductReview productReview)
        {
            
            await _appContext.ProductReviews.AddAsync(productReview);
            await _appContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ProductReview=await _appContext.ProductReviews.FirstOrDefaultAsync(x => x.Id == id);
            if (ProductReview != null)
            {

                _appContext.ProductReviews.Remove(ProductReview);
                await _appContext.SaveChangesAsync();

            }
            else {

                return;
            }
        }

        public async Task<ProductReview> GetById(int id)
        {
            return await _appContext.ProductReviews.FirstOrDefaultAsync(x=>x.Id==id);
            
        }

        public async Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId, bool onlyApproved = true)
        {
            var ProductReview = _appContext.ProductReviews
      .Where(r => r.ProductId == productId);

            if (onlyApproved)
            {
                ProductReview = ProductReview.Where(r => r.IsApproved);
            }
                return await ProductReview
                    .OrderByDescending(r => r.CreateAt)
                    .ToListAsync();
        }

        public async Task<ProductReview> UpdateAsync(ProductReview productReview)
        {
            _appContext.ProductReviews.Update(productReview);
            await _appContext.SaveChangesAsync();
            return productReview;
        }
    }
}

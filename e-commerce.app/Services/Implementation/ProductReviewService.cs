using AutoMapper;
using e_commerce.app.Dto.ProductReviewDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class ProductReviewService : IReviewProductService
    {
        private readonly IReviewProductRepo _repo;
        private readonly IMapper _mapper;
        public ProductReviewService(IReviewProductRepo _repo, IMapper _mapper)
        {
            this._repo = _repo;
            this._mapper = _mapper;
            
        }
        public async Task AddAsync(AddProductReviewDTO productReviewDTO)
        {
            var ProductReview = _mapper.Map<ProductReview>(productReviewDTO);
          await _repo.AddAsync(ProductReview);
        }

        public Task DeleteAsync(int id)
        {
          return _repo.DeleteAsync(id);
        }

        public async Task<ProductReviewDTO?> GetByIdAsync(int id)
        {
            var ProductReview=_repo.GetById(id);
            if (ProductReview == null) return null;
            return _mapper.Map<ProductReviewDTO>(ProductReview);
        }

        public async Task<IEnumerable<ProductReviewDTO>> GetByProductIdAsync(int productId, bool onlyApproved = true)
        {
            var ProductReview=await _repo.GetByProductIdAsync(productId, onlyApproved);
            return _mapper.Map<IEnumerable<ProductReviewDTO>>(ProductReview);
        }

        public async Task UpdateAsync(int reviewId, UpdateProductReviewDTO productReview)
        {
            var review = await _repo.GetById(reviewId);
            if (review == null)
            {
                return;
            }
            _mapper.Map(productReview, review);
            review.IsApproved = false;

            await _repo.UpdateAsync(review);
        }
    }
}

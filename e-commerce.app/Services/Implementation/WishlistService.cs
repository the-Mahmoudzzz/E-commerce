using AutoMapper;
using e_commerce.app.Dto.WishlistDto;
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

    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistRepository wishlistRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WishlistDto>> GetUserWishlistAsync(int userId)
        {
            var items = await _wishlistRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<WishlistDto>>(items);
        }

        public async Task<WishlistDto> AddToWishlistAsync(AddToWishlistDto dto)
        {
            var wishlist = _mapper.Map<Wishlist>(dto);
            wishlist.AddedAt = DateTime.UtcNow;
            var added = await _wishlistRepo.AddAsync(wishlist);
            return _mapper.Map<WishlistDto>(added);
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
            => await _wishlistRepo.RemoveAsync(userId, productId);

        public async Task<bool> IsProductInWishlistAsync(int userId, int productId)
            => await _wishlistRepo.ExistsAsync(userId, productId);
    }
}

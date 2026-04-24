using AutoMapper;
using e_commerce.app.Dto.WishlistDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

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
            // ✅ المنتج موجود في الـ wishlist أصلاً
            var alreadyExists = await _wishlistRepo.ExistsAsync(dto.UserId, dto.ProductId);
            if (alreadyExists)
                throw new ConflictException("This product is already in your wishlist.");

            var wishlist = _mapper.Map<Wishlist>(dto);
            wishlist.AddedAt = DateTime.UtcNow;

            var added = await _wishlistRepo.AddAsync(wishlist);
            return _mapper.Map<WishlistDto>(added);
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
        {
            // ✅ تأكد إن الـ item موجود قبل الحذف
            var exists = await _wishlistRepo.ExistsAsync(userId, productId);
            if (!exists)
                throw new NotFoundException("Wishlist item", $"userId:{userId}/productId:{productId}");

            return await _wishlistRepo.RemoveAsync(userId, productId);
        }

        public async Task<bool> IsProductInWishlistAsync(int userId, int productId)
            => await _wishlistRepo.ExistsAsync(userId, productId);
    }
}
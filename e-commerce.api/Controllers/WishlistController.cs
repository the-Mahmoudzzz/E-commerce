using e_commerce.app.Dto.WishlistDto;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var items = await wishlistService.GetUserWishlistAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToWishlistDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await wishlistService.AddToWishlistAsync(dto);
            return Created();
        }

        [HttpDelete]
        public IActionResult Remove(int userId, int productId)
        {
            wishlistService.RemoveFromWishlistAsync(userId, productId);
            return Ok();
        }
    }
}

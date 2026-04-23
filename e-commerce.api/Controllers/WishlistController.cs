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
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var items = await wishlistService.GetUserWishlistAsync(userId);
            return Ok(items);
        }

        //[HttpPost]
        //public async Task<IActionResult> Add(AddToWishlistDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    await wishlistService.AddToWishlistAsync(dto);
        //    return Created();
        //}

        [HttpPost]
        public async Task<IActionResult> Add(AddToWishlistDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await wishlistService.AddToWishlistAsync(dto);

            return Ok(new { message = "Product added to wishlist successfully", data = dto });
        }

        [HttpDelete]
        public IActionResult Remove(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            wishlistService.RemoveFromWishlistAsync(userId, productId);
            return Ok();
        }
    }
}

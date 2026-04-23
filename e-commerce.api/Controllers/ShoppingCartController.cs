using e_commerce.app.Dto.ShippingCartDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingServiece _cartService;

        public ShoppingCartController(IShoppingServiece cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("Basket")]
        public async Task<ActionResult<ShoppingCartDto>> GetUserBasket()
            
        {
            int id= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var basketDto = await _cartService.GetCartAsync(id);

            return Ok(basketDto ?? new ShoppingCartDto { Id = id });
        }
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToUserCart(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _cartService.AddItemsToCartAsync(userId, productId, quantity);

            return Ok("Item added to cart");
        }
        [HttpPut]
        public async Task<ActionResult<ShoppingCartDto>> UpdateBasket(UpdateCartDto basketDto) // بنستقبل وبنرجع DTO
        {
            // السيرفس دلوقتي هي اللي بتاخد الـ DTO وتعمل الـ Validation والمابينج وتكلم الداتا بيز
            var updatedBasket = await _cartService.UpdateCartAsync(basketDto);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketItems()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _cartService.DeleteCartItemsAsync(userId);
        }
    }
}


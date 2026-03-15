using e_commerce.app.Dto.ShippingCartDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingServiece _cartService;

        public ShoppingCartController(IShoppingServiece cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCartDto>> GetBasketById(int id) // بنرجع DTO
        {
            var basketDto = await _cartService.GetCartAsync(id);

            // لو السلة مش موجودة، بنكريتله سلة فاضية كـ DTO
            return Ok(basketDto ?? new ShoppingCartDto { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> UpdateBasket(ShoppingCartDto basketDto) // بنستقبل وبنرجع DTO
        {
            // السيرفس دلوقتي هي اللي بتاخد الـ DTO وتعمل الـ Validation والمابينج وتكلم الداتا بيز
            var updatedBasket = await _cartService.UpdateCartAsync(basketDto);
            return Ok(updatedBasket);
        }

        [HttpDelete("{id}")]
        public async Task DeleteBasket(int id)
        {
            await _cartService.DeleteCartAsync(id);
        }
    }
}


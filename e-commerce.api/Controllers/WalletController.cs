using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ISellerWalletService _service;

        public WalletController(ISellerWalletService service)
        {
            _service = service;
        }

        [HttpGet("{sellerId}")]
        [Authorize(Roles ="Seller")]
        public async Task<IActionResult> GetWallet()
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.GetWalletAsync(sellerId);
            return Ok(result);
        }

        [HttpPost("create/{sellerId}")]
        [Authorize(Roles ="Seller")]
        public async Task<IActionResult> Create()
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _service.CreateWalletIfNotExists(sellerId);
            return Ok();
        }
    }
}

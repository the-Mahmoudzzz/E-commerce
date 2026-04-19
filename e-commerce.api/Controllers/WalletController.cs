using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetWallet(int sellerId)
        {
            var result = await _service.GetWalletAsync(sellerId);
            return Ok(result);
        }

        [HttpPost("create/{sellerId}")]
        public async Task<IActionResult> Create(int sellerId)
        {
            await _service.CreateWalletIfNotExists(sellerId);
            return Ok();
        }
    }
}

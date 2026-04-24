using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Seller")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _sellerService.GetDashboardAsync();
            return Ok(result);
        }

        [HttpGet("earnings")]
        public async Task<IActionResult> GetEarnings()
        {
            var result = await _sellerService.GetEarningsAsync();
            return Ok(result);
        }
    }
}

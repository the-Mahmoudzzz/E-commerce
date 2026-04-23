using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {

            _adminService = adminService;
        }


        [HttpGet("pending-sellers")]
        public async Task<IActionResult> GetPendingSellers()
        {

            var pendingSellers = await _adminService.GetAllPandingSeller();
            return Ok(pendingSellers);
        }

        [HttpPost("approve-seller/{userId}")]
        public async Task<IActionResult> ApproveSeller(int userId)
        {
            await _adminService.ApproveSeller(userId);

            return Ok("The sealer was successfully approved.");
        }
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _adminService.GetSats();
            return Ok(result);
        }
    }
}
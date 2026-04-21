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
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("pending-sellers")]
        public async Task<IActionResult> GetPendingSellers()
        {
            var allSellers = await _userManager.
                GetUsersInRoleAsync("Seller");


            var pendingSellers = allSellers.
                Where(u => !u.IsApproved)
                .ToList();

            return Ok(pendingSellers);
        }

        [HttpPost("approve-seller/{userId}")]
        public async Task<IActionResult> ApproveSeller(int userId)
        {
            var user = await _userManager
                .FindByIdAsync(userId.ToString());

            if (user == null) 
                return NotFound("Seller Not Found");

            user.IsApproved = true;

            await _userManager.UpdateAsync(user);

            return Ok("The sealer was successfully approved.");
        }
    }
}
using e_commerce.app.Dto;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Rigester(RegisterDto register)
        {
            if (!ModelState.IsValid || register is null)
            {
                return BadRequest(ModelState);
            }
            User user = new User()
            {
                UserName = register.Name,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,

            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {

                string roleName = register.UserRole.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
                }
                await _userManager.AddToRoleAsync(user, roleName);

                return Ok(new { Message = $"User registered as {roleName} successfully" });

            }

            return BadRequest(result.Errors);
        }
    }
}

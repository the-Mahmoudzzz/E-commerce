using e_commerce.app.Dto;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SendEmailService _emailService;
        private readonly IAuthService _authService;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SendEmailService emailService,IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (!ModelState.IsValid || register == null)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = register.Name,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            string roleName = register.UserRole.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });

            await _userManager.AddToRoleAsync(user, roleName);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/account/confirm-email?email={user.Email}&code={code}";

            _emailService.SendEmail(
                user.Email,
                "Confirm your account",
                $"Hello {user.UserName},<br/><br/>Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.<br/><br/>Thank you!"
            );

            return Ok(new
            {
                Message = "User registered successfully. Check your email to confirm the account.",

            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await _authService.LogoutAsync(refreshToken);
            return Ok();
        }
    }
}

using e_commerce.app.Dto;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.Implementation;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;
using Web.App.Services;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly GetTokenServices _getTokenService;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly GoogleTokenValidator _googleTokenValidator;
        private readonly SendEmailService _emailService;
        private readonly IAuthService _authService;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, GetTokenServices getTokenService, GoogleTokenValidator googleTokenValidator, SendEmailService emailService, IAuthService authService)
        {
            _userManager = userManager;
            _getTokenService = getTokenService;
            _roleManager = roleManager;
            _emailService = emailService;
            _googleTokenValidator = googleTokenValidator;
            _authService = authService;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (!ModelState.IsValid || register == null)
                return BadRequest(ModelState);

            await _authService.RegisterAsync(register, $"{Request.Scheme}://{Request.Host}");

            return Ok("User registered successfully. Check your email.");
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



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _authService.ForgotPasswordAsync(email);
            return Ok("OTP sent to your email");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string otp, string newPassword)
        {
            await _authService.ResetPasswordAsync(email, otp, newPassword);
            return Ok("Password reset successfully");
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
        {
            var payload = await _googleTokenValidator.ValidateAsync(request.IdToken);

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);

                var loginInfo = new UserLoginInfo(
                    "Google",
                    payload.Subject,
                    "Google");

                await _userManager.AddLoginAsync(user, loginInfo);
            }

            var token = _getTokenService.GetToken(user);

            return Ok(new { token });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }
        [HttpDelete("Delete-Account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _authService.DeleteAccountAsync(userId);

            return Ok("User Deleted");


        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string Email, string code)
        {

            await _authService.ConfirmEmailAsync(Email, code);
            return Ok("Email confirmed successfully.");


        }
    }
}

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
using System.Security.Claims;
using Web.App.DTOs;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        { 
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
            var Tokengoglelogin =await _authService.GogleLogin(request);
            

            return Ok(new { Tokengoglelogin });
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

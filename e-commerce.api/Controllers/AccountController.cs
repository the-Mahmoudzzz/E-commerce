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



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Ok();

            var otp = new Random().Next(100000, 999999).ToString();

            user.ResetPasswordOTP = otp;
            user.ResetPasswordOTPExpired = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            _emailService.SendEmail(
                user.Email,
                "Reset Password OTP",
                $"Your OTP code is: <b>{otp}</b><br/>Valid for 10 minutes."
            );

            return Ok("OTP sent to your email");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User Not Found");

            if (user.ResetPasswordOTP != otp)
                return BadRequest("Invalid OTP");

            if (user.ResetPasswordOTPExpired < DateTime.UtcNow)
                return BadRequest("OTP expired");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            user.ResetPasswordOTP = null;
            user.ResetPasswordOTPExpired = null;

            await _userManager.UpdateAsync(user);

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


            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return NotFound("User Not Found");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {

                return BadRequest(result.Errors);
            }
            return Ok("User Deleted");


        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string Email, string code)
        {

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(code))
                return BadRequest("Invalid email or code.");
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }
            var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodeToken);
            if (result.Succeeded)
                return Ok("Email confirmed successfully.");

            return BadRequest("Email confirmation failed.");


        }
    }
}

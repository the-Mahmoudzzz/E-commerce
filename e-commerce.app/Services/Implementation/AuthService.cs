using e_commerce.app.Dto;
using e_commerce.app.Dto.UserDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;
using Web.App.Services;

namespace e_commerce.app.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly GetTokenServices _tokenService;
        private readonly SendEmailService _emailService;
        private readonly GoogleTokenValidator _googleTokenValidator;
        private readonly IShoppingCartRepo _cartrepo;

        public AuthService(
            UserManager<User> userManager,
            IRefreshTokenRepository refreshRepo,
            GetTokenServices tokenService,
            SendEmailService emailService,
            GoogleTokenValidator googleTokenValidator,
            
            IShoppingCartRepo cartrepo)
        {
            _userManager = userManager;
            _refreshRepo = refreshRepo;
            _tokenService = tokenService;
            _emailService = emailService;
            _googleTokenValidator = googleTokenValidator;
            _cartrepo = cartrepo;
        }

        public async Task RegisterAsync(RegisterDTO dto, string baseUrl)
        {
         

            var user = new User
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            if (dto.UserRole != UserRole.User &&
             dto.UserRole != UserRole.Seller)
            {
                throw new Exception("Invalid Role");
            }

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

           
            if (dto.UserRole == UserRole.User)
            {
                user.IsApproved = true;
                await _cartrepo.AddCatToUserAsync(user.Id);
                
            }

            await _userManager.AddToRoleAsync(user, dto.UserRole.ToString());

           

            // ابعت email تأكيد
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var link = $"{baseUrl}/api/account/confirm-email?email={user.Email}&code={code}";

            _emailService.SendEmail(
                user.Email,
                "Confirm Your Account",
                $"Click <a href='{link}'>here</a> to confirm your email.");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // FIX 2a: user مش موجود
            if (user is null )
                throw new UnauthorizedAccessException("Invalid credentials.");

            // FIX 2b: email مش مأكد
            if (!user.EmailConfirmed)
                throw new Exception("Please confirm your email before logging in.");

            if (!user.IsApproved)
                throw new Exception("Please Waiting to confirm your Account.");

            // FIX 2c: الحساب محظور من الـ Admin
            if (await _userManager.IsLockedOutAsync(user))
                throw new Exception("Account is suspended. Contact support.");

            // FIX 2d: كلمة السر غلط
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return await BuildAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> GogleLogin(GoogleLoginRequest request)
        {
            var payload = await _googleTokenValidator.ValidateAsync(request.IdToken);
            if (payload is null)
                throw new Exception("Invalid Google token.");

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                // User جديد من Google
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    EmailConfirmed = true ,
                   IsApproved = true 
                    
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                // ربط بـ Google login
                await _userManager.AddLoginAsync(user, new UserLoginInfo(
                    "Google", payload.Subject, "Google"));

                // FIX 3: حط الـ role — Google users = Customer دايماً
                await _userManager.AddToRoleAsync(user,"User" );
            }
            else
            {
                // User موجود — تأكد مش محظور
                if (await _userManager.IsLockedOutAsync(user))
                    throw new Exception("Account is suspended. Contact support.");
            }

            return await BuildAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var storedToken = await _refreshRepo.GetByTokenAsync(token);

            if (storedToken is null ||
                storedToken.Expires < DateTime.UtcNow ||
                storedToken.IsRevoked)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            // Revoke القديم
            storedToken.IsRevoked = true;

            // اعمل واحد جديد
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = storedToken.UserId
            };

            await _refreshRepo.AddAsync(newRefreshToken);

            var newAccessToken = await _tokenService.GetToken(storedToken.User);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(string token)
        {
            var storedToken = await _refreshRepo.GetByTokenAsync(token);
            if (storedToken is not null)
                await _refreshRepo.RevokeAsync(storedToken);
        }

        public async Task ConfirmEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new Exception("User not found.");

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decoded);

            if (!result.Succeeded)
                throw new Exception("Email confirmation failed.");
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return; 

            var otp = new Random().Next(100000, 999999).ToString();
            user.ResetPasswordOTP = otp;
            user.ResetPasswordOTPExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            _emailService.SendEmail(
                user.Email,
                "Reset Password OTP",
                $"OTP: <b>{otp}</b> (valid for 10 minutes)");
        }

        public async Task ResetPasswordAsync(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new Exception("User not found.");

            if (user.ResetPasswordOTP != otp ||
                user.ResetPasswordOTPExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                throw new Exception("Password reset failed.");

           
            user.ResetPasswordOTP = null;
            user.ResetPasswordOTPExpiry = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new Exception("User not found.");

            await _userManager.DeleteAsync(user);
        }

        
        private async Task<AuthResponseDto> BuildAuthResponseAsync(User user)
        {
            var accessToken = await _tokenService.GetToken(user);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _refreshRepo.AddAsync(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
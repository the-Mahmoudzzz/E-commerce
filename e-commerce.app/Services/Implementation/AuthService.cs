using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;

namespace e_commerce.app.Services.Implementation
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly GetTokenServices _tokenService;
        private readonly SendEmailService _emailService;

        public AuthService(
            UserManager<User> userManager,
            IRefreshTokenRepository refreshRepo,
            GetTokenServices tokenService, SendEmailService emailService)
        {
            _userManager = userManager;
            _refreshRepo = refreshRepo;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        public async Task RegisterAsync(RegisterDTO dto, string baseUrl)
        {
            var user = new User
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = $"{baseUrl}/api/account/confirm-email?email={user.Email}&code={code}";

            _emailService.SendEmail(
                user.Email,
                "Confirm Your Account",
                $"Click <a href='{link}'>here</a> to confirm your email");
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new UnauthorizedAccessException();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.GetToken(user);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id.ToString(),
            };

            await _refreshRepo.AddAsync(refreshToken);
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };


        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var storedToken = await _refreshRepo.GetByTokenAsync(token);

            if (storedToken == null ||
                storedToken.Expires < DateTime.UtcNow ||
                storedToken.IsRevoked)
                throw new UnauthorizedAccessException();

            storedToken.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = storedToken.UserId
            };

            await _refreshRepo.AddAsync(newRefreshToken);

            var newAccessToken = _tokenService.GetToken(storedToken.User);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(string token)
        {
            var storedToken = await _refreshRepo.GetByTokenAsync(token);
            if (storedToken != null)
                await _refreshRepo.RevokeAsync(storedToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task ConfirmEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decoded);

            if (!result.Succeeded)
                throw new Exception("Email confirmation failed");
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;

            var otp = new Random().Next(100000, 999999).ToString();
            user.ResetPasswordOTP = otp;
            user.ResetPasswordOTPExpired = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            _emailService.SendEmail(
                user.Email,
                "Reset Password OTP",
                $"OTP: <b>{otp}</b>");
        }

        public async Task ResetPasswordAsync(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            if (user.ResetPasswordOTP != otp || user.ResetPasswordOTPExpired < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                throw new Exception("Password reset failed");

            user.ResetPasswordOTP = null;
            user.ResetPasswordOTPExpired = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            await _userManager.DeleteAsync(user);
        }
    
}
}

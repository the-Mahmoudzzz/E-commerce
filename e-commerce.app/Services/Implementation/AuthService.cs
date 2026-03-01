using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
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

        public AuthService(
            UserManager<User> userManager,
            IRefreshTokenRepository refreshRepo,
            GetTokenServices tokenService)
        {
            _userManager = userManager;
            _refreshRepo = refreshRepo;
            _tokenService = tokenService;
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
    }
}

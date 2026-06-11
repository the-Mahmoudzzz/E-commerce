using e_commerce.app.Dto;
using e_commerce.app.Dto.UserDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using e_commerce.core.Exceptions;         
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
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
            // ✅ Role validation
            if (dto.UserRole != UserRole.User && dto.UserRole != UserRole.Seller)
                throw new ValidationException("UserRole", "Role must be either User or Seller.");

            // ✅ Check duplicate email
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                throw new ConflictException("An account with this email already exists.");

            var user = new User
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    throw new ValidationException(
                        "Registration failed.",
                        new Dictionary<string, string[]>
                        {
                    { "Identity", result.Errors.Select(e => e.Description).ToArray() }
                        });

                if (dto.UserRole == UserRole.User)
                {
                    user.IsApproved = true;
                    await _cartrepo.AddCatToUserAsync(user.Id); // Write 2
                }

                await _userManager.AddToRoleAsync(user, dto.UserRole.ToString()); // Write 3

                // 🚀 كل الداتا بيز تمام؟ اعتمد التغييرات
                scope.Complete();
            }

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

            // ✅ user مش موجود — نفس الرسالة عشان منكشفش إيه الغلط بالظبط
            if (user is null)
                throw new AuthenticationException("Invalid email or password.");

            // ✅ email مش متأكد
            if (!user.EmailConfirmed)
                throw new AuthenticationException("Please confirm your email before logging in.");

            // ✅ Seller لسه مش موافق عليه
            if (!user.IsApproved)
                throw new AuthenticationException("Your account is pending admin approval.");

            // ✅ الحساب محظور
            if (await _userManager.IsLockedOutAsync(user))
                throw new AuthenticationException("Account is suspended. Please contact support.");

            // ✅ باسورد غلط
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new AuthenticationException("Invalid email or password.");

            return await BuildAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> GogleLogin(GoogleLoginRequest request)
        {
            // ✅ Google token مش صحيح
            var payload = await _googleTokenValidator.ValidateAsync(request.IdToken);
            if (payload is null)
                throw new AuthenticationException("Invalid Google token. Please try again.");

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    EmailConfirmed = true,
                    IsApproved = true
                };

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                        throw new ValidationException("Google registration failed.",
new Dictionary<string, string[]>
{
    { "Identity", result.Errors.Select(e => e.Description).ToArray() }
});

                    await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
                    await _userManager.AddToRoleAsync(user, "User");

                    var response = await BuildAuthResponseAsync(user); // دي جواها إضافه للـ Refresh Token

                    scope.Complete();
                    return response;
                }
            }
            else
            {
                // ✅ موجود بس محظور
                if (await _userManager.IsLockedOutAsync(user))
                    throw new AuthenticationException("Account is suspended. Please contact support.");
            }

            return await BuildAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var storedToken = await _refreshRepo.GetByTokenAsync(token);

            // ✅ توكن مش موجود، منتهي، أو مسحوب
            if (storedToken is null || storedToken.IsRevoked)
                throw new AuthenticationException("Invalid refresh token.");

            if (storedToken.Expires < DateTime.UtcNow)
                throw new AuthenticationException("Refresh token has expired. Please log in again.");

            // Revoke القديم واعمل واحد جديد
            storedToken.IsRevoked = true;

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
            // ✅ لو التوكن مش موجود — مش error، بس نخرج بهدوء
            var storedToken = await _refreshRepo.GetByTokenAsync(token);
            if (storedToken is not null)
                await _refreshRepo.RevokeAsync(storedToken);
        }

        public async Task ConfirmEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new NotFoundException("User", email);

            // ✅ لو الإيميل متأكد أصلاً
            if (user.EmailConfirmed)
                throw new BusinessRuleException("Email is already confirmed.");

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decoded);

            if (!result.Succeeded)
                throw new BusinessRuleException("Email confirmation failed. The link may have expired.");
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // ✅ مش بنكشف إن الإيميل ده مش موجود — security best practice
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
                throw new NotFoundException("User", email);

            // ✅ OTP غلط أو منتهي
            if (user.ResetPasswordOTP != otp)
                throw new BusinessRuleException("Invalid OTP. Please request a new one.");

            if (user.ResetPasswordOTPExpiry < DateTime.UtcNow)
                throw new BusinessRuleException("OTP has expired. Please request a new one.");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                    throw new ValidationException("Password reset failed.",
                     new Dictionary<string, string[]>
                         {
                      { "Password", result.Errors.Select(e => e.Description).ToArray() }
                       });

                user.ResetPasswordOTP = null;
                user.ResetPasswordOTPExpiry = null;
                await _userManager.UpdateAsync(user);

                scope.Complete();
            }
        }

        public async Task DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("User", userId);

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
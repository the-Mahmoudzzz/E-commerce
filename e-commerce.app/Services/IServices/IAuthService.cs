using e_commerce.app.Dto;
using Web.App.DTOs;

namespace e_commerce.app.Services.IServices
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto, string baseUrl);
        Task<AuthResponseDto> LoginAsync(LoginDTO dto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<AuthResponseDto> GogleLogin(GoogleLoginRequest request);
        Task LogoutAsync(string token);
        Task ConfirmEmailAsync(string email, string code);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string email, string otp, string newPassword);
        Task DeleteAccountAsync(string userId);
    }
}

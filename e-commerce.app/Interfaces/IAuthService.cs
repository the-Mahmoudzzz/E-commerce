using e_commerce.app.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;

namespace Web.App.Intarfeces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDTO dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task RegisterAsync(RegisterDTO dto, string baseUrl);
  
        Task ConfirmEmailAsync(string email, string code);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string email, string otp, string newPassword);
        Task DeleteAccountAsync(string userId);
    }
}

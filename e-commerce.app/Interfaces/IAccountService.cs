using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;
using e_commerce.core.entities;
namespace Web.App.Intarfeces
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDTO dto, string baseUrl);
        Task<string> LoginAsync(LoginDTO dto);
        Task ConfirmEmailAsync(string email, string code);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string email, string otp, string newPassword);
        Task DeleteAccountAsync(string userId);
    }
}

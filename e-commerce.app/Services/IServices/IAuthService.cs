using e_commerce.app.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.App.DTOs;

namespace e_commerce.app.Services.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDTO dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}

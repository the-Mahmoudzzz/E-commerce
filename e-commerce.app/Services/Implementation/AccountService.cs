using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.Implementation;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Web.App.DTOs;
using Web.App.Intarfeces;


namespace Web.App.Services
{
    public class AccountService : IAccountService
    {
        //private readonly UserManager<User> _userManager;
        //private readonly GetTokenServices _tokenService;
        //private readonly SendEmailService _emailService;

        //public AccountService(
        //    UserManager<User> userManager,
        //    GetTokenServices tokenService,
        //    SendEmailService emailService)
        //{
        //    _userManager = userManager;
        //    _tokenService = tokenService;
        //    _emailService = emailService;
        //}



        //public async Task<string> LoginAsync(LoginDTO dto)
        //{
        //    var user = await _userManager.FindByEmailAsync(dto.Email);
        //    if (user == null || !user.EmailConfirmed)
        //        throw new Exception("User Name or pass is wrong");

        //    var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        //    if (!isValid)
        //        throw new Exception("User Name or pass is wrong");

        //    return _tokenService.GetToken(user);
        //}

    }     
}

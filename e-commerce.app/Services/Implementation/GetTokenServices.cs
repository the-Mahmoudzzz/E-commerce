using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class GetTokenServices:ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey symmetricSecurityKey;
        private readonly UserManager<User> _userManager;
        public GetTokenServices(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            _userManager = userManager;
        }

        public async Task<string> GetToken(User user)

        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Name,user.UserName),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim (JwtRegisteredClaimNames.NameId,user.Id.ToString()),
            };
            foreach (var role in roles)
            {
               claims.Add (new Claim(ClaimTypes.Role, role));
            }
            
            var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescrptor = new SecurityTokenDescriptor {
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.Now.AddDays(1),
                Audience = configuration["JWT:Audiance"],
                Issuer = configuration["JWT:Issuar"],
                SigningCredentials = creds
            
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescrptor);
            return TokenHandler.WriteToken(token);


        }
    }
}

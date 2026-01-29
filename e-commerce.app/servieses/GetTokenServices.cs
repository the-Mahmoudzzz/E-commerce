using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using e_commerce.core.entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace e_commerce.app.servieses
{
    public class GetTokenServices:ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey symmetricSecurityKey;
        public GetTokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.symmetricSecurityKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])) ;
        }

        public string GetToken(User user)
        {
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Name,user.UserName),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),

            };
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

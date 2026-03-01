using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.App.Services
{
    public class GoogleTokenValidator
    {
        public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>
            {
                "568455379940-c1j3200v2rt42vkejo6i6pn7ilv6uv4r.apps.googleusercontent.com"
            }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
    }
}

using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app
{
 interface ITokenService
    {
        public string GetToken(User user);
    }
}

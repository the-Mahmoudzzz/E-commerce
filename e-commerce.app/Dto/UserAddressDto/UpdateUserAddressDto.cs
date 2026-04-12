using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.UserAddressDto
{
    public class UpdateUserAddressDto
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}

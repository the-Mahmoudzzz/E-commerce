using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.OrderDto
{
    public class OrderCreateDto
    {
        public int CartId { get; set; }

        public int USerAddressId { get; set; }
        public int ShippingZoneId { get; set; }

    }
}

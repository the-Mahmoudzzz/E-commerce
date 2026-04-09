using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ShipmentDTO
{
    public class ShipmentCreateDto
    {
        public string CourierName { get; set; }
        public int TrackingNumber { get; set; }
        public int OrderId { get; set; }
        public int? ShippingZoneId { get; set; }
    }
}

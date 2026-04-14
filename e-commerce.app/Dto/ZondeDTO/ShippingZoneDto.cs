using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ZondeDTO
{
    public class ShippingZoneDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public decimal ShippingCost { get; set; }
        public int EstimatedDays { get; set; }
    }
}

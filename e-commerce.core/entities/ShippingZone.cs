using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class ShippingZone
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public decimal ShipingCost { get; set; }
        public int EstimatedDays { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual ICollection<User> Customers { get; set; }
    }
}

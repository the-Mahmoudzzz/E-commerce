using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class Shipment
    {
        public int Id { get; set; }
        public string CourierName { get; set; }
        public int TrackingNumber { get; set; }
        public DateTime ShippedDate{get;set;}
        public DateTime DelevierdDate{get;set;}

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order order { get; set; }
    }
}

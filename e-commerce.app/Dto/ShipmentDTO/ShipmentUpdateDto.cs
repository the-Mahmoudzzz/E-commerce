using e_commerce.core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ShipmentDTO
{
    public class ShipmentUpdateDto
    {
        public string? CourierNamel;
        public int? TrackingNumber;
        public ShipmentStatus Status;
        public DateTime? DeliveredDate;
    }
}

using e_commerce.core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.PayMentDTO
{
    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}

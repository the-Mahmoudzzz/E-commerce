using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.PayMentDTO
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public string PaymentUrl { get; set; } = null!;
    }
}

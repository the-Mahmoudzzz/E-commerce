using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.WirhDrawlsDTO
{
    public class CreateWithdrawalDto
    {
        public int SellerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDetails { get; set; } = null!;
    }
}

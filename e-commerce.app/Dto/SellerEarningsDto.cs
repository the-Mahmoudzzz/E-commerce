using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto
{
    public class SellerEarningsDto
    {
        public decimal TotalEarnings { get; set; }
        public List<EarningTransactionDto> Transactions { get; set; }
    }

    public class EarningTransactionDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

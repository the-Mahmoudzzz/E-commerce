using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public enum WithdrawlsStatus
    {
        Pending,
        Processing,
        Shipped,
        Canceled,
        Refused
    }

    public class Withdrawal
    {
        public int Id { get; set; }
        
        public int SelerId { get; set; }
        [ForeignKey("SelerId")]
        public User Seler { get; set; }
        public decimal Amount { get; set; }
        public WithdrawlsStatus WithdrawlsStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public string PaymentDetails { get; set; }

    }
}

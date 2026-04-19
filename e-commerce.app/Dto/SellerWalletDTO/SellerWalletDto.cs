using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.SellerWalletDTO
{
    public class SellerWalletDto
    {
        public int SellerId { get; set; }
        public decimal Balance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal LifeTimeEarnings { get; set; }
    }
}

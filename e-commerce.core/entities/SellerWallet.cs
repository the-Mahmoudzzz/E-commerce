using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class SellerWallet
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public int LifeTimeEarnings { get; set; }

    }
}

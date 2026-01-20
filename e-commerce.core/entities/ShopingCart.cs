using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class ShopingCart
    {
        public int Id { get; set; }
        public int CustmerId { get; set; }
        [ForeignKey("CustmerId")]
        public User Custmoer { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<ShoppingCartItem> Items { get; set; }
    }
}

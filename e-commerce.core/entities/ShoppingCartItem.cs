using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class ShoppingCartItem
    {
        public int Id { get; set; } 

        public int ShoppingCartId { get; set; } 
        public virtual ShopingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; } 

        public decimal PriceAtTime { get; set; }
    }
}

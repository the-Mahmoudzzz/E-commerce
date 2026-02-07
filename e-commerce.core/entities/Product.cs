using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public User Seller { get; set; } = null!;
        public int CreatedByAdminId { get; set; }
        [ForeignKey("CreatedByAdminId")]
        
        public User CreatedByAdmin { get; set; } = null!;
        public virtual ICollection<ShoppingCartItem> Items { get; set; }



    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class User:IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } 

        public DateTime? UpdateddAt { get; set; } 
        public ICollection<Product> Products { get; set; } = new List<Product>();

        
        [InverseProperty(nameof(Product.CreatedByAdmin))]
        public ICollection<Product> CreatedProducts { get; set; } = new List<Product>();
        [InverseProperty(nameof(Product.Seller))]
        public ICollection<Product> SoldProducts { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; }= new List<Order>();

        public ICollection<Category> CreatedCategories { get; set; } = new List<Category>();
        public ShopingCart shopingCart { get; set; }
        public int? ShippingZoneId { get; set; } 

        [ForeignKey("ShippingZoneId")]
        public virtual ShippingZone ShippingZone { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<UserAddresse> UserAddresses { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; }

        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual SellerWallet? SellerWallet { get; set; }


    }
}

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
        public ICollection<Category> CreatedCategories { get; set; } = new List<Category>();
        public ShopingCart shopingCart { get; set; }
        public int ShippingZoneId { get; set; } 

        [ForeignKey("ShippingZoneId")]
        public virtual ShippingZone ShippingZone { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<UserAddresse> UserAddresses { get; set; }



    }
}

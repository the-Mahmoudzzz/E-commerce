using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; }

    }
}

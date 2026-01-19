using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        public String  DiscountType { get; set; }
        public decimal Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinOrderAmount { get; set; }
        public bool  IsActive {  get; set; }
        public int sellerId { get; set; }

        public ICollection<DiscountCategry> DiscountCategries { get; set; } = new List<DiscountCategry>();
    }
}

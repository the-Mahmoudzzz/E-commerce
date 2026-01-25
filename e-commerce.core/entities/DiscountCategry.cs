using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class DiscountCategry
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        public virtual Discount Discount { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}

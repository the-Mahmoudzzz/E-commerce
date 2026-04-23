using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ProductDto
{
    public class UpdateProductBySellerDto
    {
        public string? Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }

        public bool? IsActive { get; set; } //admin only


    }
}

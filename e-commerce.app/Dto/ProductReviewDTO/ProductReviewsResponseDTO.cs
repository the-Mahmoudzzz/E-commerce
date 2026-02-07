using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ProductReviewDTO
{
    public class ProductReviewsResponseDTO
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }

        public List<ProductReviewDTO> Reviews { get; set; }
    }
}

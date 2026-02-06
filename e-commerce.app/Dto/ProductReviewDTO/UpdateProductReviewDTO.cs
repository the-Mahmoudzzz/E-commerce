using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ProductReviewDTO
{
    public class UpdateProductReviewDTO
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        public string ReviewText { get; set; }
    }
}

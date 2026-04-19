using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto
{
    public class CreateDiscountDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string DiscountType { get; set; }

        [Required]
        public decimal Value { get; set; }

        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal MinOrderAmount { get; set; }

        public int SellerId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto
{
    public class ApplyDiscountDto
    {

        [Required]
        public string Code { get; set; }

        [Required]
        public decimal OrderTotal { get; set; }
    }
}

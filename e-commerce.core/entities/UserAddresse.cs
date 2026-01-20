using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class UserAddresse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}

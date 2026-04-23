using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ShippingCartDTO
{
    public class UpdateCartDto
    {
        public int Id { get; set; }
        public List<UpdateCartItemDto> Items { get; set; } = new();
    }
}

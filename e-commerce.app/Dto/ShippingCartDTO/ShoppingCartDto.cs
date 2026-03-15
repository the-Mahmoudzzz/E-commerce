using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.ShippingCartDTO
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public List<ShoppingCartItemDto>Items { get; set; }=new List<ShoppingCartItemDto>();
    }
}

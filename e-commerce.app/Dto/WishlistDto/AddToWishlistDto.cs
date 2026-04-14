using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.WishlistDto
{
    public class AddToWishlistDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}

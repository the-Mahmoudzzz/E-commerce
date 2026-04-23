using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.SellerDTO
{
   
        public class SellerDashboardDto
        {
            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
            public List<TopProductDto> TopProducts { get; set; }
        }

        public class TopProductDto
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public int SoldQuantity { get; set; }
        }
    
}

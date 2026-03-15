using e_commerce.app.Dto.OrderDto;
using e_commerce.app.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class OrderServiece : IOrderService
    {
        public Task CreateOrder(OrderCreateDto order)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<OrderDTO>> GetOrderByCustomerId(int customeid)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetOrderById(int orderid)
        {
            throw new NotImplementedException();
        }
    }
}

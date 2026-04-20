using e_commerce.app.Dto.OrderDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IOrderService
    {
        Task CreateOrder(OrderCreateDto order);
        Task <OrderDTO> GetOrderById(int orderid);
        Task<IReadOnlyList<OrderDTO>> GetOrderByCustomerId(int customeid);
        Task<IEnumerable<OrderDTO>> GetIncomingOrder();

    }
}

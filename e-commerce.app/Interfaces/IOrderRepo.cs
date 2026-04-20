using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.core.entities;

namespace e_commerce.app.Interfaces
{
    public interface IOrderRepo
    {
        Task CreateOrderAsync( Order orderCreateDto);
        Task<Order> GetOrderById(int orderId);

       Task<IReadOnlyList<Order>> GetOrderByUser(int Customerid);
        Task UpdateOrder(Order order);
        Task<IEnumerable<Order>> GetIncomingOrder(int sellerid);
    }
}

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

        Task<Order> GetOrdersForUserAsync(int customerId);
    }
}

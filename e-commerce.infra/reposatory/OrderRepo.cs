using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.infra.reposatory
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateOrderAsync(Order orderCreateDto)
        {
            await _context.orders.AddAsync(orderCreateDto);
            await _context.SaveChangesAsync();
           
        }
       
        public async Task<Order> GetOrderById(int orderId)

           => await _context.orders.Include(o => o.OrderDetails).
            ThenInclude(p=>p.Product)
            .Include (y=>y.Payment).
            Include(s=>s.Shipment) 
            .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<IReadOnlyList<Order>> GetOrderByUser(int Customerid)
        {
            return await _context.orders
           .Include(o => o.OrderDetails).ThenInclude(p=>p.Product)
           .Where(o => o.CustomerId == Customerid && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Refused)
           .OrderByDescending(o => o.CreatedAt) 
           .ToListAsync();

        }

        public async Task UpdateOrder(Order order)
        {
           _context.orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>>GetIncomingOrder(int sellerid)
        {
            return   _context.orders.Include(o=>o.Customer).Include(o=>o.OrderDetails)
                .ThenInclude(o=>o.Product).Where(o => o.OrderDetails.Any(d => d.Product.SellerId == sellerid))
            .OrderByDescending(o=>o.CreatedAt);
            
            
        }

        public async Task<int> GetCountOrder()
        {
           return await _context.orders
              .Where(o => o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Refused)
                 .CountAsync();
        }

        public async Task<decimal> GetTotalCount()
        {
            return await _context.orders
             .Where(o => o.Status == OrderStatus.Shipped)
               .SumAsync(o => o.FinalAmount);
        }
    }
}

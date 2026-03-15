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
           .Include(o => o.OrderDetails)
           .Where(o => o.CustomerId == Customerid)
           .OrderByDescending(o => o.CreatedAt) 
           .ToListAsync();

        }
    }
}

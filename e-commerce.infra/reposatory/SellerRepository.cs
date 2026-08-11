using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class SellerRepository : ISellerRepository
    {
        private readonly AppDbContext _context;

        public SellerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetSellerOrdersAsync(int sellerId,PaginationParamsDto pagination)
        {
            return await _context.orders.AsNoTracking()
                .Where(o => o.OrderDetails.Any(d => d.Product.SellerId == sellerId))
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product).OrderBy(s => s.Id).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> GetSellerOrderDetailsAsync(int sellerId,PaginationParamsDto pagination)
        {
            return await _context.orderDetails.AsNoTracking()
                .Where(d => d.Product.SellerId == sellerId)
                .Include(d => d.Product).OrderBy(f => f.Id).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize)
                .ToListAsync();
        }

        
    }
}

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
    public class DiscountRepo:IDiscountRepo
    {
        private readonly AppDbContext _context;

        public DiscountRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Discount> GetByIdAsync(int id)
        {
            return await _context.Discounts
                .Include(d => d.DiscountCategries)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IReadOnlyList<Discount>> GetAllAsync(PaginationParamsDto pagination)
        {
            return await _context.Discounts.AsNoTracking()
                .Include(d => d.DiscountCategries).OrderBy(d=>d.Id).Skip((pagination.PageNumber-1)*pagination.PageSize).Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<Discount> GetActiveDiscountByCodeAsync(string code)
        {
            var now = DateTime.UtcNow;

            return await _context.Discounts
                .FirstOrDefaultAsync(d =>
                    d.Code == code &&
                    d.IsActive &&
                    d.StartDate <= now &&
                    d.EndDate >= now);
        }

        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                discount.IsActive = false; // Soft Delete
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Discounts
                .AnyAsync(d => d.Code.ToLower() == code.ToLower());
        }
    }
}


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
    public class WithdrawalRepository : IWithdrawalRepository
    {
        private readonly AppDbContext _context;

        public WithdrawalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Withdrawal> AddAsync(Withdrawal withdrawal)
        {
            _context.withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();
            return withdrawal;
        }

        public async Task<Withdrawal?> GetByIdAsync(int id)
        {
            return await _context.withdrawals.FindAsync(id);
        }

        public async Task<List<Withdrawal>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.withdrawals
                .Where(x => x.SelerId == sellerId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Withdrawal withdrawal)
        {
            _context.withdrawals.Update(withdrawal);
            await _context.SaveChangesAsync();
        }
    }
}

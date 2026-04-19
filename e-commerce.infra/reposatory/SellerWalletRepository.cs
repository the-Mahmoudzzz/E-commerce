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
    public class SellerWalletRepository : ISellerWalletRepository
    {
        private readonly AppDbContext _context;
      

        public SellerWalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellerWallet?> GetBySellerIdAsync(int sellerId)
        {
            return await _context.SellerWallets
                .FirstOrDefaultAsync(x => x.SellerId == sellerId);
        }

        public async Task AddAsync(SellerWallet wallet)
        {
            _context.SellerWallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SellerWallet wallet)
        {
            _context.SellerWallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}

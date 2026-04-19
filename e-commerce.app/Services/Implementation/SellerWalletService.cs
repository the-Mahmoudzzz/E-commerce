using e_commerce.app.Dto.SellerWalletDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class SellerWalletService : ISellerWalletService
    {
        private readonly ISellerWalletRepository _repo;

        public SellerWalletService(ISellerWalletRepository repo)
        {
            _repo = repo;
        }

        public async Task<SellerWalletDto> GetWalletAsync(int sellerId)
        {
            var wallet = await _repo.GetBySellerIdAsync(sellerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            return new SellerWalletDto
            {
                SellerId = wallet.SellerId,
                Balance = wallet.Balance,
                PendingBalance = wallet.PendingBalance,
                LifeTimeEarnings = wallet.LifeTimeEarnings
            };
        }

        public async Task CreateWalletIfNotExists(int sellerId)
        {
            var wallet = await _repo.GetBySellerIdAsync(sellerId);

            if (wallet != null) return;

            await _repo.AddAsync(new SellerWallet
            {
                SellerId = sellerId,
                Balance = 0,
                PendingBalance = 0,
                LifeTimeEarnings = 0
            });
        }

     
        public async Task AddPendingBalance(int sellerId, decimal amount)
        {
            var wallet = await _repo.GetBySellerIdAsync(sellerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            wallet.PendingBalance += amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }

      
        public async Task ConfirmPayment(int sellerId, decimal amount)
        {
            var wallet = await _repo.GetBySellerIdAsync(sellerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.PendingBalance < amount)
                throw new Exception("Invalid pending amount");

            wallet.PendingBalance -= amount;
            wallet.Balance += amount;
            wallet.LifeTimeEarnings += amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }

   
        public async Task DeductForWithdrawal(int sellerId, decimal amount)
        {
            var wallet = await _repo.GetBySellerIdAsync(sellerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.Balance < amount)
                throw new Exception("Insufficient balance");

            wallet.Balance -= amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }
    }
}

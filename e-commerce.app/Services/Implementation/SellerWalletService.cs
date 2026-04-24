using e_commerce.app.Dto.SellerWalletDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

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
                throw new NotFoundException("Wallet", sellerId);

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
            if (wallet != null) return;  // ✅ موجودة أصلاً — مش error

            await _repo.AddAsync(new SellerWallet
            {
                SellerId = sellerId,
                Balance = 0,
                PendingBalance = 0,
                LifeTimeEarnings = 0,
                LastUpdated = DateTime.UtcNow
            });
        }

        public async Task AddPendingBalance(int sellerId, decimal amount)
        {
            // ✅ Validate amount
            if (amount <= 0)
                throw new ValidationException("Amount", "Amount must be greater than zero.");

            var wallet = await _repo.GetBySellerIdAsync(sellerId);
            if (wallet == null)
                throw new NotFoundException("Wallet", sellerId);

            wallet.PendingBalance += amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }

        public async Task ConfirmPayment(int sellerId, decimal amount)
        {
            if (amount <= 0)
                throw new ValidationException("Amount", "Amount must be greater than zero.");

            var wallet = await _repo.GetBySellerIdAsync(sellerId);
            if (wallet == null)
                throw new NotFoundException("Wallet", sellerId);

            // ✅ Pending balance أقل من المبلغ — data inconsistency
            if (wallet.PendingBalance < amount)
                throw new BusinessRuleException(
                    $"Pending balance ({wallet.PendingBalance:C}) is less than the amount to confirm ({amount:C}).");

            wallet.PendingBalance -= amount;
            wallet.Balance += amount;
            wallet.LifeTimeEarnings += amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }

        public async Task DeductForWithdrawal(int sellerId, decimal amount)
        {
            if (amount <= 0)
                throw new ValidationException("Amount", "Amount must be greater than zero.");

            var wallet = await _repo.GetBySellerIdAsync(sellerId);
            if (wallet == null)
                throw new NotFoundException("Wallet", sellerId);

            if (wallet.Balance < amount)
                throw new BusinessRuleException(
                    $"Insufficient balance. Available: {wallet.Balance:C}, Requested: {amount:C}.");

            wallet.Balance -= amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(wallet);
        }
    }
}
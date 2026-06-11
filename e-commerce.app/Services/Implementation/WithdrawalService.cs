using e_commerce.app.Dto.WirhDrawlsDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;
using System.Transactions; // ← ضيف ده

namespace e_commerce.app.Services.Implementation
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IWithdrawalRepository _withdrawalRepo;
        private readonly ISellerWalletRepository _walletRepo;

        public WithdrawalService(
            IWithdrawalRepository withdrawalRepo,
            ISellerWalletRepository walletRepo)
        {
            _withdrawalRepo = withdrawalRepo;
            _walletRepo = walletRepo;
        }

        public async Task<WithdrawalResponseDto> RequestWithdrawalAsync(int sellerId, CreateWithdrawalDto dto)
        {
            if (dto.Amount <= 0)
                throw new ValidationException("Amount", "Withdrawal amount must be greater than zero.");

            var wallet = await _walletRepo.GetBySellerIdAsync(sellerId);
            if (wallet == null)
                throw new NotFoundException("Wallet", sellerId);

            if (wallet.Balance < dto.Amount)
                throw new BusinessRuleException(
                    $"Insufficient balance. Available: {wallet.Balance:C}, Requested: {dto.Amount:C}.");

            var pendingWithdrawal = await _withdrawalRepo.GetBySellerIdAsync(sellerId);
            if (pendingWithdrawal != null)
                throw new BusinessRuleException(
                    "You already have a pending withdrawal request. Please wait for it to be processed.");

            var withdrawal = new Withdrawal
            {
                SelerId = sellerId,
                Amount = dto.Amount,
                WithdrawlsStatus = WithdrawlsStatus.Pending,
                RequestDate = DateTime.UtcNow,
                PaymentDetails = dto.PaymentDetails
            };

            
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                wallet.Balance -= dto.Amount;
                wallet.PendingBalance += dto.Amount;
                wallet.LastUpdated = DateTime.UtcNow;

                await _walletRepo.UpdateAsync(wallet);
                await _withdrawalRepo.AddAsync(withdrawal);

                
                scope.Complete();
            }

            return new WithdrawalResponseDto
            {
                Id = withdrawal.Id,
                Amount = withdrawal.Amount,
                Status = withdrawal.WithdrawlsStatus
            };
        }

        public async Task ApproveWithdrawalAsync(int withdrawalId)
        {
            var withdrawal = await _withdrawalRepo.GetByIdAsync(withdrawalId);
            if (withdrawal == null)
                throw new NotFoundException("Withdrawal", withdrawalId);

            if (withdrawal.WithdrawlsStatus != WithdrawlsStatus.Pending)
                throw new BusinessRuleException(
                    $"Withdrawal has already been {withdrawal.WithdrawlsStatus}.");

            var wallet = await _walletRepo.GetBySellerIdAsync(withdrawal.SelerId);
            if (wallet == null)
                throw new NotFoundException("Wallet", withdrawal.SelerId);

            if (wallet.PendingBalance < withdrawal.Amount)
                throw new BusinessRuleException(
                    $"Pending balance ({wallet.PendingBalance:C}) is less than withdrawal amount ({withdrawal.Amount:C}).");

            
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                wallet.PendingBalance -= withdrawal.Amount;
                wallet.LastUpdated = DateTime.UtcNow;
                await _walletRepo.UpdateAsync(wallet);

                withdrawal.WithdrawlsStatus = WithdrawlsStatus.Paid;
                await _withdrawalRepo.UpdateAsync(withdrawal);

                
                scope.Complete();
            }
        }
    }
}
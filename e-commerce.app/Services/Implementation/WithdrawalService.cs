using e_commerce.app.Dto.WirhDrawlsDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;

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


        public async Task<WithdrawalResponseDto> RequestWithdrawalAsync(CreateWithdrawalDto dto)
        {
            var wallet = await _walletRepo.GetBySellerIdAsync(dto.SellerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (dto.Amount <= 0)
                throw new Exception("Invalid amount");

            if (wallet.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

         
            wallet.Balance -= dto.Amount;
            wallet.PendingBalance += dto.Amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _walletRepo.UpdateAsync(wallet);

            var withdrawal = new Withdrawal
            {
                SelerId = dto.SellerId,
                Amount = dto.Amount,
                WithdrawlsStatus = WithdrawlsStatus.Pending,
                RequestDate = DateTime.UtcNow,
                PaymentDetails = dto.PaymentDetails
            };

            await _withdrawalRepo.AddAsync(withdrawal);

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
                throw new Exception("Withdrawal not found");

            if (withdrawal.WithdrawlsStatus != WithdrawlsStatus.Pending)
                throw new Exception("Already processed");

            var wallet = await _walletRepo.GetBySellerIdAsync(withdrawal.SelerId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.PendingBalance < withdrawal.Amount)
                throw new Exception("Invalid pending balance");

            wallet.PendingBalance -= withdrawal.Amount;
            wallet.LastUpdated = DateTime.UtcNow;

            await _walletRepo.UpdateAsync(wallet);

            withdrawal.WithdrawlsStatus = WithdrawlsStatus.Paid;

            await _withdrawalRepo.UpdateAsync(withdrawal);
        }
    }
}
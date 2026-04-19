using e_commerce.app.Dto.SellerWalletDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface ISellerWalletService
    {
        Task<SellerWalletDto> GetWalletAsync(int sellerId);
        Task CreateWalletIfNotExists(int sellerId);
        Task AddPendingBalance(int sellerId, decimal amount);
        Task ConfirmPayment(int sellerId, decimal amount);
        Task DeductForWithdrawal(int sellerId, decimal amount);
    }
}

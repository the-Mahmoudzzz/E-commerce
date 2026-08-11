using e_commerce.app.Dto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface ISellerWalletRepository
    {
        Task<SellerWallet?> GetBySellerIdAsync(int sellerId);
        Task AddAsync(SellerWallet wallet);
        Task UpdateAsync(SellerWallet wallet);
    }
}

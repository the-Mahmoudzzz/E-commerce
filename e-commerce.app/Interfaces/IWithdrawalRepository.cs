using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    
    public interface IWithdrawalRepository
    {
        Task<Withdrawal> AddAsync(Withdrawal withdrawal);
        Task<Withdrawal?> GetByIdAsync(int id);
        Task<List<Withdrawal>> GetBySellerIdAsync(int sellerId);
        Task UpdateAsync(Withdrawal withdrawal);
    }
}

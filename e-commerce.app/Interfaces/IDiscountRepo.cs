using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IDiscountRepo
    {
        Task<Discount> GetByIdAsync(int id);

        Task<IReadOnlyList<Discount>> GetAllAsync();

        Task<Discount> GetActiveDiscountByCodeAsync(string code);

        Task AddAsync(Discount discount);

        Task UpdateAsync(Discount discount);

        Task DeleteAsync(int id);
    }
}

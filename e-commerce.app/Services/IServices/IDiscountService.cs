using e_commerce.app.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IDiscountService
    {
        Task<DiscountDto> ApplyDiscountAsync(string code, decimal orderTotal);
        Task<IReadOnlyList<DiscountDto>> GetAllAsync();
        Task<DiscountDto> GetByIdAsync(int id);
        Task AddAsync(DiscountDto dto);
        Task UpdateAsync(DiscountDto dto);
        Task DeleteAsync(int id);
    }
}

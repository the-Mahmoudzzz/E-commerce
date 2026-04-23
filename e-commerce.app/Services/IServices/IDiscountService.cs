using e_commerce.app.Dto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IDiscountService
    {
        Task<Discount> ApplyDiscountAsync(string code, decimal orderTotal);
        Task<IReadOnlyList<DiscountDto>> GetAllAsync();
        Task<DiscountDto> GetByIdAsync(int id);
        Task AddAsync(CreateDiscountDto dto);
        Task UpdateAsync(UpdateDiscountDto dto);
        Task DeleteAsync(int id);
    }
}

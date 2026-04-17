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
        Task<CreateDiscountDto> ApplyDiscountAsync(string code, decimal orderTotal);
        Task<IReadOnlyList<CreateDiscountDto>> GetAllAsync();
        Task<CreateDiscountDto> GetByIdAsync(int id);
        Task AddAsync(CreateDiscountDto dto);
        Task UpdateAsync(CreateDiscountDto dto);
        Task DeleteAsync(int id);
    }
}

using e_commerce.app.Dto.CtegoriesDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface ICategoryService
    {
        Task AddAsync(CreateCategoryDto category);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetbyIdAsync(int id);
        Task<Category> UpdateAsync(CreateCategoryDto category);
        Task DeleteAsync(int id);
    }
}

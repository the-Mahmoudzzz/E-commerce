using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
     public interface ICategoryRepo
    {
        Task AddAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetAllSubAsync();
        Task<Category> GetbyIdAsync(int id);
        Task<Category> GetbyIdSubAsync(int id);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}

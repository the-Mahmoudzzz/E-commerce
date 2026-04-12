using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<UserAddresse>> GetByUserIdAsync(int userId);
        Task<UserAddresse> GetByIdAsync(int addressId);
        Task<UserAddresse> AddAsync(UserAddresse address);
        Task<UserAddresse> UpdateAsync(UserAddresse address);
        Task<bool> DeleteAsync(int addressId);
        Task ResetDefaultAsync(int userId);
    }
}

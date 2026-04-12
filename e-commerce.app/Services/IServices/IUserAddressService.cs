using e_commerce.app.Dto.UserAddressDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IUserAddressService
    {
        Task<IEnumerable<UserAddressDto>> GetUserAddressesAsync(int userId);
        Task<UserAddressDto> GetAddressByIdAsync(int userId, int addressId);
        Task<UserAddressDto> CreateAddressAsync(int userId, CreateUserAddressDto dto);
        Task<UserAddressDto> UpdateAddressAsync(int userId, int addressId, UpdateUserAddressDto dto);
        Task<bool> DeleteAddressAsync(int userId, int addressId);
        Task<bool> SetDefaultAddressAsync(int userId, int addressId);
    }
}

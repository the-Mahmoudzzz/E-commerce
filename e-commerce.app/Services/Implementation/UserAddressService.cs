using AutoMapper;
using e_commerce.app.Dto.UserAddressDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IMapper _mapper;

        public UserAddressService(IUserAddressRepository addressRepo, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _addressRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserAddressDto>>(addresses);
        }

        public async Task<UserAddressDto> GetAddressByIdAsync(int userId, int addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);
            if (address == null || address.CustomerId != userId) return null;
            return _mapper.Map<UserAddressDto>(address);
        }

        public async Task<UserAddressDto> CreateAddressAsync(int userId, CreateUserAddressDto dto)
        {
            if (dto.IsDefault)
                await _addressRepo.ResetDefaultAsync(userId);

            var address = _mapper.Map<UserAddresse>(dto);
            address.CustomerId = userId;

            var created = await _addressRepo.AddAsync(address);
            return _mapper.Map<UserAddressDto>(created);
        }

        public async Task<UserAddressDto> UpdateAddressAsync(int userId, int addressId, UpdateUserAddressDto dto)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);
            if (address == null || address.CustomerId != userId) return null;

            if (dto.IsDefault)
                await _addressRepo.ResetDefaultAsync(userId);

            _mapper.Map(dto, address);
            var updated = await _addressRepo.UpdateAsync(address);
            return _mapper.Map<UserAddressDto>(updated);
        }

        public async Task<bool> DeleteAddressAsync(int userId, int addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);
            if (address == null || address.CustomerId != userId) return false;
            return await _addressRepo.DeleteAsync(addressId);
        }

        public async Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);
            if (address == null || address.CustomerId != userId) return false;

            await _addressRepo.ResetDefaultAsync(userId);
            address.IsDefault = true;
            await _addressRepo.UpdateAsync(address);
            return true;
        }
    }
}

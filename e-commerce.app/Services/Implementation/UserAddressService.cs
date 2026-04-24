using AutoMapper;
using e_commerce.app.Dto.UserAddressDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

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

            // ✅ مش موجودة أو مش بتاعت الـ user ده
            if (address == null)
                throw new NotFoundException("Address", addressId);

            if (address.CustomerId != userId)
                throw new UnauthorizedException("You are not authorized to access this address.");

            return _mapper.Map<UserAddressDto>(address);
        }

        public async Task<UserAddressDto> CreateAddressAsync(int userId, CreateUserAddressDto dto)
        {
            // ✅ لو default، نشيل الـ default القديم أولاً
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

            if (address == null)
                throw new NotFoundException("Address", addressId);

            if (address.CustomerId != userId)
                throw new UnauthorizedException("You are not authorized to update this address.");

            if (dto.IsDefault)
                await _addressRepo.ResetDefaultAsync(userId);

            _mapper.Map(dto, address);

            var updated = await _addressRepo.UpdateAsync(address);
            return _mapper.Map<UserAddressDto>(updated);
        }

        public async Task<bool> DeleteAddressAsync(int userId, int addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);

            if (address == null)
                throw new NotFoundException("Address", addressId);

            if (address.CustomerId != userId)
                throw new UnauthorizedException("You are not authorized to delete this address.");

            // ✅ منعاش تحذف الـ default address لو في غيرها
            if (address.IsDefault)
            {
                var allAddresses = await _addressRepo.GetByUserIdAsync(userId);
                if (allAddresses.Count() > 1)
                    throw new BusinessRuleException(
                        "Cannot delete the default address. Please set another address as default first.");
            }

            return await _addressRepo.DeleteAsync(addressId);
        }

        public async Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);

            if (address == null)
                throw new NotFoundException("Address", addressId);

            if (address.CustomerId != userId)
                throw new UnauthorizedException("You are not authorized to modify this address.");

            // ✅ هي default أصلاً
            if (address.IsDefault)
                throw new BusinessRuleException("This address is already set as the default.");

            await _addressRepo.ResetDefaultAsync(userId);
            address.IsDefault = true;
            await _addressRepo.UpdateAsync(address);

            return true;
        }
    }
}
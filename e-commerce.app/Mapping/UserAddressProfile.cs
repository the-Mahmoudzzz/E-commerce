using AutoMapper;
using e_commerce.app.Dto.UserAddressDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class UserAddressProfile : Profile
    {
        public UserAddressProfile()
        {
            CreateMap<UserAddresse, UserAddressDto>();
            CreateMap<CreateUserAddressDto, UserAddresse>();
            CreateMap<UpdateUserAddressDto, UserAddresse>();
        }
    }
}

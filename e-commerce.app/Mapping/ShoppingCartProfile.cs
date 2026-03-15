using AutoMapper;
using e_commerce.app.Dto.ShippingCartDTO;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class ShoppingCartProfile:Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShopingCart, ShoppingCartDto>();
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>();

            CreateMap<ShoppingCartDto, ShopingCart>();
            CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
        }
    }
}

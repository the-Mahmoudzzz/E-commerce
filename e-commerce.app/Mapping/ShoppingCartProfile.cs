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

            // التعديل السحري هنا: بنفهم AutoMapper يجيب الداتا منين بالظبط
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
           
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            CreateMap<ShoppingCartDto, ShopingCart>();
            CreateMap<ShoppingCartItemDto, ShoppingCartItem>();


            CreateMap<UpdateCartItemDto, ShoppingCartItem>()
            .ForMember(dest => dest.PriceAtTime, opt => opt.Ignore())
           .ForMember(dest => dest.Product, opt => opt.Ignore()) 
            .ForMember(dest => dest.ShoppingCartId, opt => opt.Ignore()); ; 

            CreateMap<UpdateCartDto, ShopingCart>()
                .ForMember(dest => dest.CustmerId, opt => opt.Ignore())   
                .ForMember(dest => dest.Custmoer, opt => opt.Ignore());
        }
    }
}

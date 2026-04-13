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
                // استخدم السطر ده لو عايز السعر دايماً يكون متحدث (Live Price)
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                // أو لو كنت عايز تستخدم PriceAtTime، امسح السطر اللي فوق واستخدم ده:
                // .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceAtTime))

                // بونص: لو الـ Product جواه مسار الصورة، هتحتاج تعملها مابينج هي كمان كدا
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            CreateMap<ShoppingCartDto, ShopingCart>();
            CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
        }
    }
}

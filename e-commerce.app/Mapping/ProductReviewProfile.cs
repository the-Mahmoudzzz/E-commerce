using AutoMapper;
using e_commerce.core.entities;
using e_commerce.app.Dto.ProductReviewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class ProductReviewProfile:Profile
    {
        public ProductReviewProfile()
        {
           
            CreateMap<ProductReview, ProductReviewDTO>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));

            
            CreateMap<AddProductReviewDTO, ProductReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            
            CreateMap<UpdateProductReviewDTO, ProductReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());
        }
    }
}

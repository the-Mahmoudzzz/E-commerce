using AutoMapper;
using e_commerce.core.entities;
using e_commerce.app.Dto;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {

        CreateMap<CreateDiscountDto, Discount>();
        CreateMap<UpdateDiscountDto, Discount>();
        CreateMap<Discount, DiscountDto>();
    }
}
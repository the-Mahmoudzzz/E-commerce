using AutoMapper;
using e_commerce.core.entities;
using e_commerce.app.Dto;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        
        CreateMap<Discount, DiscountDto>().ReverseMap();
    }
}
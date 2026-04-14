using AutoMapper;
using e_commerce.app.Dto.ZondeDTO;
using e_commerce.core.entities;

public class ShipingZoneProfile : Profile
{
    public ShipingZoneProfile()
    {
        CreateMap<ShippingZone, ShippingZoneDto>()
            .ForMember(dest => dest.ShippingCost,
                       opt => opt.MapFrom(src => src.ShipingCost));

        CreateMap<ShippingZoneDto, ShippingZone>()
            .ForMember(dest => dest.ShipingCost,
                       opt => opt.MapFrom(src => src.ShippingCost));
    }
}
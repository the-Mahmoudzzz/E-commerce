using AutoMapper;
using e_commerce.app.Dto.OrderDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            
            CreateMap<Order, OrderDTO>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

           
            CreateMap<OrderDetail, OrderItemDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s.PriceAtTime))
                
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));
        }
    }
}

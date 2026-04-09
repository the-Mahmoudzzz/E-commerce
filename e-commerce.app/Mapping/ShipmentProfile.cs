using AutoMapper;
using e_commerce.app.Dto.ShipmentDTO;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class ShipmentProfile:Profile
    {
        public ShipmentProfile()
        {
            CreateMap<Shipment, ShipmentDto>();

            // DTO → Entity
            CreateMap<ShipmentCreateDto, Shipment>();
        }
    }
}

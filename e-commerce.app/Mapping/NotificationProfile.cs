using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class NotificationProfile:Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotifcationDto>();
            CreateMap<CreateNotificationDto, Notification>().ForMember(u => u.UserId, o => o.MapFrom(u => u.UserId));

               
        }
    }
}

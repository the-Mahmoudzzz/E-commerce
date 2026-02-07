using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using e_commerce.app.Dto.FeedBackDTO;
using e_commerce.core.entities;
namespace e_commerce.app.Mapping
{
    public class FeedbackProfile:Profile
    {
        public FeedbackProfile()
        {
            CreateMap<CreateFeedbackDto, Feedback>();
            CreateMap<Feedback, FeedBackDTO>().ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName));
        }

    }
}

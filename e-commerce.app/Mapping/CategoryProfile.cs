using AutoMapper;
using e_commerce.app.Dto.CtegoriesDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap< Category, CategoryDto>();
            CreateMap< CreateCategoryDto, Category>();
        }
    }
}

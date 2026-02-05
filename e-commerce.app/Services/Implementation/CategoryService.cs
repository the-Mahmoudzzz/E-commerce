using AutoMapper;
using e_commerce.app.Dto.CtegoriesDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo repo;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepo repo ,IMapper mapper)
        {
            this.repo = repo;

         this.mapper = mapper;
            
        }

       

        async Task ICategoryService.AddAsync(CreateCategoryDto categoryDto)
        {
           var category= mapper.Map<Category>(categoryDto);
            await repo.AddAsync(category);
        }

        async Task ICategoryService.DeleteAsync(int id)
        {
           await repo.DeleteAsync(id);
        }

        async Task<IEnumerable<CategoryDto>> ICategoryService.GetAllAsync()
        {
            var categories=await repo.GetAllAsync();
            return  mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        async Task<CategoryDto> ICategoryService.GetbyIdAsync(int id)
        {
            var category=await repo.GetbyIdAsync(id);
            return mapper.Map<CategoryDto>(category);
        }

        async Task<Category> ICategoryService.UpdateAsync(CreateCategoryDto category)
        {
            var updateCategory=mapper.Map<Category>(category);
           return await repo.UpdateAsync(updateCategory);

        }
    }
}

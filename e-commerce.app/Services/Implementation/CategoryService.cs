using AutoMapper;
using e_commerce.app.Dto.CtegoriesDto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.Cashe;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo repo;
        private readonly IMapper mapper;
        private readonly IRedisCahse _redis;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CategoryService(ICategoryRepo repo, IMapper mapper, IRedisCahse redis, IHttpContextAccessor httpContextAccessor)
        {
            this.repo = repo;

            this.mapper = mapper;
            _redis = redis;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new AuthenticationException("User is not authenticated.");

            return int.Parse(userIdClaim);
        }

        async Task ICategoryService.AddAsync(CreateCategoryDto categoryDto)
        {
           var category= mapper.Map<Category>(categoryDto);
            await repo.AddAsync(category);
        }

        async Task ICategoryService.AddSubCategoryAsync(CreateSubCategoryDto category)
        {
            var subcategory = mapper.Map<Category>(category);
            await repo.AddAsync(subcategory);
        }
       

        async Task ICategoryService.DeleteAsync(int id)
        {
           await repo.DeleteAsync(id);
        }

        async Task<IEnumerable<CategoryDto>> ICategoryService.GetAllAsync()
        {
            // 1. خلينا الـ Key عام لكل الناس مش مربوط بيوزر معين
            string cacheKey = "Categories_All";

            // 2. ظبطنا الـ Type عشان يرجع IEnumerable
            var cachedData = await _redis.GetTData<IEnumerable<CategoryDto>>(cacheKey);

            if (cachedData is not null && cachedData.Any())
            {
                return cachedData;
            }

            // 3. لو مش في الكاش، هاته من الداتا بيز
            var categories = await repo.GetAllAsync();

            // 4. اعمل الـ Mapping مرة واحدة بس
            var mappedCategories = mapper.Map<IEnumerable<CategoryDto>>(categories);

            
             _redis.SetData(cacheKey, mappedCategories);

            return mappedCategories;
        }

        async Task<IEnumerable<SubCategoryDto>> ICategoryService.GetAllSubCategoryAsync()
        {
            var categories = await repo.GetAllSubAsync();
            return mapper.Map<IEnumerable<SubCategoryDto>>(categories);
        }

        async Task<CategoryDto> ICategoryService.GetbyIdAsync(int id)
        {
            var category=await repo.GetbyIdAsync(id);
            return mapper.Map<CategoryDto>(category);
        }

        async Task<SubCategoryDto> ICategoryService.GetbyIdSubCategoryAsync(int id)
        {
            var category = await repo.GetbyIdSubAsync(id);
            return mapper.Map<SubCategoryDto>(category);
        }

        async Task<Category> ICategoryService.UpdateAsync(CreateCategoryDto category)
        {
            var updateCategory=mapper.Map<Category>(category);
           return await repo.UpdateAsync(updateCategory);

        }

       async Task<Category> ICategoryService.UpdateSubCategoryAsync(CreateSubCategoryDto category)
        {
            var updateCategory = mapper.Map<Category>(category);
            return await repo.UpdateAsync(updateCategory);
        }
    }
}

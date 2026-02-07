using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;


namespace e_commerce.infra.reposatory
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext con;

        public CategoryRepo( AppDbContext con)
        {
            this.con = con;
        }
        async Task ICategoryRepo.AddAsync(Category category)
        {
           await con.AddAsync(category);
            await con.SaveChangesAsync();
            
        }
      

        async Task ICategoryRepo.DeleteAsync(int id)
        {
            var cate=con.categories.FirstOrDefault(c => c.Id==id);
            if(cate!=null)
            {
                con.categories.Remove(cate);
                await con.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<Category>> ICategoryRepo.GetAllAsync()
        {
            return await con.categories.ToListAsync();
        }

        async Task<IEnumerable<Category>> ICategoryRepo.GetAllSubAsync()
        {
            return await con.categories.Include(s=>s.ParentCategory).Where(s=>s.ParentCategoryId!=null).ToListAsync();
        }

        Task<Category> ICategoryRepo.GetbyIdAsync(int id)
        {
            return con.categories.FirstOrDefaultAsync(c=>c.Id==id);
        }

        Task<Category> ICategoryRepo.GetbyIdSubAsync(int id)
        {
            return con.categories.Include(s=>s.ParentCategory).FirstOrDefaultAsync(c => c.Id == id && c.ParentCategoryId!=null) ;
        }

        async Task<Category> ICategoryRepo.UpdateAsync(Category category)
        {
            con.Update(category);
            await con.SaveChangesAsync();
            return category;
        }
    }
}

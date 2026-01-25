using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.Data
{
    public class AppDbContext:IdentityDbContext<User,IdentityRole<int> ,int>
    {
        public AppDbContext (DbContextOptions<AppDbContext> dbContext):base(dbContext) {}
       
        public DbSet<User> users { get; set; }
       
    }
}

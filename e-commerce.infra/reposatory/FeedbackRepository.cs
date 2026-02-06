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
    public class FeedbackRepository : IFeedBackRepo
    {
        private readonly AppDbContext appDbContext;
        public FeedbackRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            
        }
        public async Task AddAsync(Feedback feedback)
        {
           await appDbContext.Feedbacks.AddAsync(feedback);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
           return await appDbContext.Feedbacks.Include(f=>f.Customer).OrderByDescending(f=>f.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByTypeAsync(FeedbackType feedbackType)
        {
            return await appDbContext.Feedbacks.Include(f => f.Customer).Where(f => f.Type == feedbackType).OrderByDescending(f => f.CreatedAt).ToListAsync();
        }
    }
}

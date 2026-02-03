using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services
{
    public interface IFeedBackRepo
    {
        Task AddAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<IEnumerable<Feedback>> GetByTypeAsync(FeedbackType feedbackType);
    }
}

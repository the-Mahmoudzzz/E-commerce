using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
     interface ICategoryRepo
    {
        Task AddAsync(Category feedback);
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<IEnumerable<Feedback>> GetByTypeAsync(FeedbackType feedbackType);
    }
}

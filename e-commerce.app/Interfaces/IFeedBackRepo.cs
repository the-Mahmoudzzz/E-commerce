using e_commerce.app.Dto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IFeedBackRepo
    {
        Task AddAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetAllAsync(PaginationParamsDto pagination);
        Task<IEnumerable<Feedback>> GetByTypeAsync(FeedbackType feedbackType);
    }
}

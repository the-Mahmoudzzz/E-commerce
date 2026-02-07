using e_commerce.app.Dto.FeedBackDTO;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IFeedbackService
    {
        Task CreateAsync(CreateFeedbackDto createFeedbackDto);
        Task<IEnumerable<FeedBackDTO>> GetAllAsync();
        Task<IEnumerable<FeedBackDTO>> GetByTypeAsync(FeedbackType feedbackType);
    }
}

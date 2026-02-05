using AutoMapper;
using e_commerce.app.Dto.FeedBackDTO;
using e_commerce.app.Services;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace e_commerce.app.Services.Implementation
{
    public class FeedBackService : IFeedbackService
    {
        private readonly IFeedBackRepo feedBackRepo;
        private readonly IMapper mapper;
        public FeedBackService(IFeedBackRepo feedBackRepo,IMapper mapper)
        {
            this.feedBackRepo = feedBackRepo;
            this.mapper = mapper;
        }
        public async Task CreateAsync(CreateFeedbackDto createFeedbackDto)
        {
         
        
           
            var feedBack = mapper.Map<Feedback>(createFeedbackDto);

            await feedBackRepo.AddAsync(feedBack);
           
        }

        public async Task<IEnumerable<FeedBackDTO>> GetAllAsync()
        {
           var feedBacks = await feedBackRepo.GetAllAsync();
            return mapper.Map<IEnumerable<FeedBackDTO>>(feedBacks);

        }

        public async Task<IEnumerable<FeedBackDTO>> GetByTypeAsync(FeedbackType feedbackType)
        {
            var feedBacks = await feedBackRepo.GetByTypeAsync(feedbackType);
            return mapper.Map<IEnumerable<FeedBackDTO>>(feedBacks);
        }
    }
}

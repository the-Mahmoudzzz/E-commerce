using AutoMapper;
using e_commerce.app.Dto.FeedBackDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

namespace e_commerce.app.Services.Implementation
{
    public class FeedBackService : IFeedbackService
    {
        private readonly IFeedBackRepo _feedBackRepo;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepo feedBackRepo, IMapper mapper)
        {
            _feedBackRepo = feedBackRepo;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateFeedbackDto dto)
        {
            // ✅ Validate message مش فاضي
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ValidationException("Message", "Feedback message cannot be empty.");

            // ✅ Validate طول الرسالة
            if (dto.Message.Length > 1000)
                throw new ValidationException("Message", "Feedback message cannot exceed 1000 characters.");

            var feedback = _mapper.Map<Feedback>(dto);
            await _feedBackRepo.AddAsync(feedback);
        }

        public async Task<IEnumerable<FeedBackDTO>> GetAllAsync()
        {
            var feedbacks = await _feedBackRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<FeedBackDTO>>(feedbacks);
        }

        public async Task<IEnumerable<FeedBackDTO>> GetByTypeAsync(FeedbackType feedbackType)
        {
            // ✅ Validate إن الـ enum value صحيح
            if (!Enum.IsDefined(typeof(FeedbackType), feedbackType))
                throw new ValidationException("FeedbackType", "Invalid feedback type.");

            var feedbacks = await _feedBackRepo.GetByTypeAsync(feedbackType);
            return _mapper.Map<IEnumerable<FeedBackDTO>>(feedbacks);
        }
    }
}
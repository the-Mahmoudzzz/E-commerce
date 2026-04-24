using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.External;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace e_commerce.app.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotifiRepo _notifiRepo;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;   // ← ضيف logger

        public NotificationService(
            INotifiRepo notifiRepo,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext,
            ILogger<NotificationService> logger)
        {
            _notifiRepo = notifiRepo;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task AddNotifiAsync(CreateNotificationDto dto)
        {
            // ✅ Validate المحتوى
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ValidationException("Message", "Notification message cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Title", "Notification title cannot be empty.");

            var notification = _mapper.Map<Notification>(dto);
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;

            // ✅ بنحفظ في الـ DB أولاً — ده الـ source of truth
            await _notifiRepo.AddNotifiAsync(notification);

            // ✅ SignalR best-effort — لو فشل مش هنكرش الـ notification
            try
            {
                await _hubContext.Clients
                    .User(notification.UserId.ToString())
                    .SendAsync("ReceiveNotification", new
                    {
                        notification.Id,
                        notification.Title,
                        notification.Message,
                        notification.CreatedAt,
                        notification.IsRead
                    });
            }
            catch (Exception ex)
            {
                // ✅ نسجل الـ warning بس — مش error critical
                // اليوزر هيلاقي الـ notification لما يفتح التطبيق
                _logger.LogWarning(ex,
                    "SignalR delivery failed for user {UserId}. Notification saved to DB.",
                    notification.UserId);
            }
        }

        public async Task Delete(int id)
        {
            // ✅ تأكد إن الـ notification موجودة قبل الحذف
            var notification = await _notifiRepo.GetByidAsync(id);
            if (notification == null)
                throw new NotFoundException("Notification", id);

            await _notifiRepo.Delete(id);
        }

        public async Task<IEnumerable<NotifcationDto>> GetALLAsync()
        {
            var notifications = await _notifiRepo.GetALLAsync();
            return _mapper.Map<IEnumerable<NotifcationDto>>(notifications);
        }

        public async Task<IEnumerable<NotifcationDto>> GetALLUserNotifiAsync(int userId)
        {
            if (userId <= 0)
                throw new ValidationException("UserId", "Invalid user ID.");

            var notifications = await _notifiRepo.GetALLUserNotifiAsync(userId);
            return _mapper.Map<IEnumerable<NotifcationDto>>(notifications);
        }

        public async Task<NotifcationDto> GetByidAsync(int id)
        {
            var notification = await _notifiRepo.GetByidAsync(id);

            // ✅ بدل ما نرجع null
            if (notification == null)
                throw new NotFoundException("Notification", id);

            return _mapper.Map<NotifcationDto>(notification);
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            if (userId <= 0)
                throw new ValidationException("UserId", "Invalid user ID.");

            await _notifiRepo.MarkAllAsReadAsync(userId);
        }

        public async Task MarkAsReadAsync(int userId, int id)
        {
            if (userId <= 0)
                throw new ValidationException("UserId", "Invalid user ID.");

            var notification = await _notifiRepo.GetByidAsync(id);
            if (notification == null)
                throw new NotFoundException("Notification", id);

            // ✅ تأكد إن الـ notification بتاعت الـ user ده فعلاً
            if (notification.UserId != userId)
                throw new UnauthorizedException("You cannot mark another user's notification as read.");

            await _notifiRepo.MarkAsReadAsync(userId, id);
        }
    }
}
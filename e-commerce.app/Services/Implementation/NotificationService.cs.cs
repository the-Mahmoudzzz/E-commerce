using AutoMapper;
using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.External;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotifiRepo notifiRepo;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(INotifiRepo notifiRepo, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            this.notifiRepo = notifiRepo;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        public async Task AddNotifiAsync(CreateNotificationDto notification)
        {
            var notifi = _mapper.Map<Notification>(notification);
            await notifiRepo.AddNotifiAsync(notifi);
            await _hubContext.Clients.User(notifi.UserId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    notifi.Id,
                    notifi.Message,
                    notifi.CreatedAt
                });
        }


        public async Task Delete(int id)
        {
            await notifiRepo.Delete(id);
        }

        public async Task<IEnumerable<NotifcationDto>> GetALLAsync()
        {
            var allnotifi = await notifiRepo.GetALLAsync();
            return  _mapper.Map<IEnumerable<NotifcationDto>>(allnotifi);
        }

        public async Task<IEnumerable<NotifcationDto>> GetALLUserNotifiAsync(int userid)
        {
            var allnotifi = await notifiRepo.GetALLUserNotifiAsync(userid);
            return _mapper.Map<IEnumerable<NotifcationDto>>(allnotifi);
        }

        public async Task<NotifcationDto> GetByidAsync(int id)
        {
           var notifi=await notifiRepo.GetByidAsync(id);
            return _mapper.Map<NotifcationDto>(notifi);
        }

        public async Task MarkAllAsReadAsync(int userid)
        {
            await notifiRepo.MarkAllAsReadAsync(userid);
        }

        public async Task MarkAsReadAsync(int userid,int id)
        {
            await notifiRepo.MarkAsReadAsync(userid,id);
        }
    }
}

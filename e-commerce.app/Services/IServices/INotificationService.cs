using e_commerce.app.Dto.NotificationDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotifcationDto>> GetALLAsync();
        Task<IEnumerable<NotifcationDto>> GetALLUserNotifiAsync(int userid);
        Task<NotifcationDto> GetByidAsync(int id);
        Task AddNotifiAsync(CreateNotificationDto notification);
        Task Delete(int id);
        Task MarkAsReadAsync(int userid,int id);
        Task MarkAllAsReadAsync(int userid);

    }
}

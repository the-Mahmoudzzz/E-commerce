using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface INotifiRepo
    {
        Task <IEnumerable<Notification>> GetALLAsync();
        Task <IEnumerable<Notification>> GetALLUserNotifiAsync(int userid);
        Task<Notification> GetByidAsync(int id);
        Task AddNotifiAsync(Notification notification);
        Task Delete (int id);
        Task MarkAsReadAsync(int userid, int id);
        Task MarkAllAsReadAsync(int userid);

    }
}

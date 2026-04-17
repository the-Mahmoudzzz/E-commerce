using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class NotitfiRepo : INotifiRepo
    {
        private readonly AppDbContext _con;

        public NotitfiRepo(AppDbContext con)
        {
            _con = con;
        }

        public async Task AddNotifiAsync(Notification notification)
        {
            await _con.AddAsync(notification);
            await _con.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var delete =await _con.notifications.FindAsync(id);
            if (delete != null)
            {
                _con.Remove(delete);
                _con.SaveChanges();
            }
        }

        public async Task<IEnumerable<Notification>> GetALLAsync()
        {
            return await _con.notifications.ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetALLUserNotifiAsync(int userid)
        {
            return await _con.notifications.Where(u=>u.UserId==userid)
                .OrderByDescending(n=>n.CreatedAt)
                .ToListAsync();
        }

        public Task<Notification> GetByidAsync(int id)
        {
            return _con.notifications.FirstOrDefaultAsync(n=>n.Id==id);
        }

        public async Task MarkAllAsReadAsync(int userid)
        {
            await _con.notifications
            .Where(n => n.UserId == userid && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
        }

        public async Task MarkAsReadAsync(int userid, int id)
        {
            var notification = await _con.notifications.FirstOrDefaultAsync(n=>n.UserId==userid&&n.Id==id);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                _con.notifications.Update(notification);
                await _con.SaveChangesAsync();
            }
        }
    }
}

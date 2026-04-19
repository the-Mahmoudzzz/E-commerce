using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.External
{
    public  class NotificationHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            // UserId بيجي من الـ JWT claim تلقائياً
            await base.OnConnectedAsync();
        }
    }
}

using e_commerce.app.Dto.PayMentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IPaymobService
    {
        Task<string> GetAuthToken();
        Task<int> CreateOrder(string token, decimal amount);
        Task<string> GetPaymentKey(string token, int orderId, decimal amount);
        Task <bool>ValidateHmac(PaymobCallbackDto callback, string receivedHmac);
    }
}

using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<Payment?> GetByTransactionRef(string reference);
        Task UpdateAsync(Payment payment);
    }
}

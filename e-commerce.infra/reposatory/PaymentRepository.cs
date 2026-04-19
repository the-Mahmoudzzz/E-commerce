using e_commerce.app.Interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _context.payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment?> GetByTransactionRef(string reference)
    {
        return await _context.payments
            .FirstOrDefaultAsync(p => p.TransactionReference == reference);
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}
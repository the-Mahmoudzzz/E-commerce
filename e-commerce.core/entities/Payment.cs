using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.core.entities
{
    public enum PaymentStatus
    {
        Pending,
        Approved,
        Refused,
        Refunded
    }

    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        public string PaymentMethod { get; set; } = string.Empty;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? TransactionReference { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
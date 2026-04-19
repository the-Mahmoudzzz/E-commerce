using System;
using System.ComponentModel.DataAnnotations.Schema;
using e_commerce.core.Enum;
namespace e_commerce.core.entities
{


    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        //public string? ProviderResponse { get; set; } 

        //public string? ErrorMessage { get; set; }
        public string? TransactionReference { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
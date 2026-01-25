using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.core.entities
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Canceled,
        Refused
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } 

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }

        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment? Payment { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual Shipment? Shipment { get; set; }
    }
}
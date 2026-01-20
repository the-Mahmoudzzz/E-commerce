using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.core.entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAtTime { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
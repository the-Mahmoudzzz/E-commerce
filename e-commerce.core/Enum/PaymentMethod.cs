using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.Enum
{
    public enum PaymentMethod
    {
        CashOnDelivery,
        Card,
        Wallet
    }

    public enum PaymentStatus
    {
        Pending,
        Approved,
        Refused,
        Cancelled
    }
}
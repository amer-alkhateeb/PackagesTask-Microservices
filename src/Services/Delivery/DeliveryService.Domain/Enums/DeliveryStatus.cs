using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Enums
{
    public enum DeliveryStatus
    {
        Scheduled = 0,
        OutForDelivery = 1,
        Delivered = 2,
        Failed = 3
    }
}

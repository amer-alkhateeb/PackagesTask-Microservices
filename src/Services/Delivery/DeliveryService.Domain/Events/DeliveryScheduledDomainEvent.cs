using DeliveryService.Domain.Abstraction;
using DeliveryService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Events
{
    public sealed record DeliveryScheduledDomainEvent(
        RouteId RouteId,
        DeliveryId DeliveryId,
        string PackageId,
        DateTime EstimatedTime
    ) : IDomainEvent;
}

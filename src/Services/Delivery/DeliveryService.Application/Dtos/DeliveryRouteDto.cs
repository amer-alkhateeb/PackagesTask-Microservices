using DeliveryService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.Dtos
{
    public sealed record DeliveryRouteDto(
        Guid Id,
        Guid TruckId,
        Guid DriverId,
        DateTime ScheduledDate,
        IReadOnlyList<DeliveryDto> Deliveries
    )
    {
        public static DeliveryRouteDto From(DeliveryRoute route) =>
            new(
                route.Id.Value,
                route.TruckId.Value,
                route.DriverId.Value,
                route.ScheduleDate,
                route.Deliveries.Select(d => new DeliveryDto(
                    d.Id.Value,
                    d.PackageId,
                    d.Status.ToString(),
                    d.TrackingCode)).ToList()
            );
    }

    public sealed record DeliveryDto(Guid Id, string PackageId, string Status, string TrackingCode);
}

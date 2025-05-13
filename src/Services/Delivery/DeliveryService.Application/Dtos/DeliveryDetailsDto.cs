using DeliveryService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.Dtos
{
    public sealed record DeliveryDetailsDto(
        Guid Id,
        string PackageId,
        string Status,
        string TrackingCode,
        DateTime EstimatedTime,
        DateTime? ActualTime
    )
    {
        public static DeliveryDetailsDto From(Delivery delivery) =>
            new(
                delivery.Id.Value,
                delivery.PackageId,
                delivery.Status.ToString(),
                delivery.TrackingCode,
                delivery.EstimatedTime,
                delivery.ActualTime
            );
    }
}

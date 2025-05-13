using DeliveryService.Domain.Abstraction;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObjects;

namespace DeliveryService.Domain.Models
{
    public sealed class Delivery : Entity<DeliveryId>
    {
        public string PackageId { get; private set; } // Packages Service
        public DeliveryStatus Status { get; private set; }
        public DateTime EstimatedTime { get; private set; }
        public DateTime? ActualTime { get; private set; }
        public string TrackingCode { get; private set; }

        private Delivery() { }

        private Delivery (DeliveryId id , string packageId, DeliveryStatus status, DateTime estimatedTime)
        {
            Id = id;
            PackageId = packageId;
            Status = status;
            EstimatedTime = estimatedTime;
            TrackingCode = Guid.NewGuid().ToString()[..8].ToUpper();
        }
        public static Delivery Schedule(string packageId, DateTime estimatedTime)
        {
            return new Delivery(DeliveryId.Create(), packageId,DeliveryStatus.Scheduled, estimatedTime);
        }
        public void MarkDelivered(DateTime completedAt)
        {
            Status = DeliveryStatus.Delivered;
            ActualTime = completedAt;
        }

        public void FailDelivery()
        {
            Status = DeliveryStatus.Failed;
        }
    }
}

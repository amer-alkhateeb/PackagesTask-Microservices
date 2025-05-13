using DeliveryService.Domain.Abstraction;
using DeliveryService.Domain.Events;
using DeliveryService.Domain.ValueObjects;

namespace DeliveryService.Domain.Models
{
    public sealed class DeliveryRoute : Aggregate<RouteId>
    {
        public DateTime ScheduleDate { get; private set; }
        public TruckId TruckId { get; private set; }
        public DriverId DriverId { get; private set; }

        private readonly List<Delivery> _deliveries = new();
        public IReadOnlyCollection<Delivery> Deliveries => _deliveries.AsReadOnly();

        private DeliveryRoute() { }

        private DeliveryRoute(RouteId id, TruckId truckId, DriverId driverId, DateTime scheduleDate)
        {
            Id = id;
            ScheduleDate = scheduleDate;
            TruckId = truckId;
            DriverId = driverId;
        }

        public static DeliveryRoute Schedule(RouteId id, TruckId truckId, DriverId driverId, DateTime date)
        {
            return new DeliveryRoute(id, truckId, driverId, date);
        }

        public void AddDelivery(Delivery delivery)
        {
            _deliveries.Add(delivery);
            AddDomainEvent(new DeliveryScheduledDomainEvent(
                RouteId: Id,
                DeliveryId: delivery.Id,
                PackageId: delivery.PackageId,
                EstimatedTime: delivery.EstimatedTime
            ));
        }
    }
}

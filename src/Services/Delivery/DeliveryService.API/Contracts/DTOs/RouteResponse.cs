namespace DeliveryService.API.Contracts.DTOs
{
    public sealed record RouteResponse(
        Guid Id,
        Guid TruckId,
        Guid DriverId,
        DateTime ScheduledDate,
        List<DeliverySummary> Deliveries
    );

    public sealed record DeliverySummary(Guid Id, string PackageId, string Status, string TrackingCode);
}

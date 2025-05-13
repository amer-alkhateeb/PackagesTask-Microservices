namespace DeliveryService.API.Contracts.DTOs
{
    public sealed record CreateRouteRequest(
        Guid TruckId,
        Guid DriverId,
        List<string> PackageIds,
        DateTime ScheduledDate
    );
}

namespace DeliveryService.Application.DeliveryRoutes.Events
{
    public sealed record DeliveryScheduledMessage(
        Guid RouteId,
        Guid DeliveryId,
        string PackageId,
        DateTime EstimatedTime
    );
}

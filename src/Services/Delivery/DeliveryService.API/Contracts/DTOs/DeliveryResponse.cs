namespace DeliveryService.API.Contracts.DTOs
{
    public sealed record DeliveryResponse(
        Guid Id,
        string PackageId,
        string Status,
        string TrackingCode,
        DateTime EstimatedTime,
        DateTime? ActualTime
    );
}

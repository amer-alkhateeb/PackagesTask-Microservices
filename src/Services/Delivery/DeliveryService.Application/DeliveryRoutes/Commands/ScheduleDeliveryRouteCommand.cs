using DeliveryService.Application.CQRS;
namespace DeliveryService.Application.DeliveryRoutes.Commands
{
    public sealed record ScheduleDeliveryRouteCommand(Guid TruckId,
    Guid DriverId,
    List<string> PackageIds,
    DateTime ScheduledDate) : ICommand<ScheduleDeliveryRouteCommandResponse>;
    public sealed record ScheduleDeliveryRouteCommandResponse (Guid Id);
}

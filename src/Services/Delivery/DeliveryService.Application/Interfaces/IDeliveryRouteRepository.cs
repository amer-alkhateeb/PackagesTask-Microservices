using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;
namespace DeliveryService.Application.Interfaces
{
    public interface IDeliveryRouteRepository : IRepository<DeliveryRoute, RouteId> {
        Task<Delivery?> GetByTrackingCodeAsync(string trackingCode, CancellationToken cancellationToken);
    }
}
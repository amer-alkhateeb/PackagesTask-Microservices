using DeliveryService.Application.CQRS;
using DeliveryService.Application.Dtos;

namespace DeliveryService.Application.DeliveryRoutes.Queries
{
    public sealed record GetDeliveryByTrackingCodeQuery(string TrackingCode) : IQuery <GetDeliveryByTrackingCodeQueryResponse>
    {
    }

    public sealed record GetDeliveryByTrackingCodeQueryResponse (DeliveryDetailsDto DeliveryDetails)
    {
    }
}

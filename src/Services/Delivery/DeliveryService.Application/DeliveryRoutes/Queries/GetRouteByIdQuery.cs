using DeliveryService.Application.CQRS;
using DeliveryService.Application.Dtos;

namespace DeliveryService.Application.DeliveryRoutes.Queries
{
    public sealed record GetRouteByIdQuery (Guid Id) : IQuery<GetRouteByIdQueryResponse>
    {
    }public sealed record GetRouteByIdQueryResponse (DeliveryRouteDto route);
}

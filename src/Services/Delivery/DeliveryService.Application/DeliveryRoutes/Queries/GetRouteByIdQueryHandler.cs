using DeliveryService.Application.CQRS;
using DeliveryService.Application.Dtos;
using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.ValueObjects;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.DeliveryRoutes.Queries
{
    public sealed class GetRouteByIdQueryHandler : IQueryHandler<GetRouteByIdQuery, GetRouteByIdQueryResponse>
    {
        private readonly IDeliveryRouteRepository _repository;

        public GetRouteByIdQueryHandler(IDeliveryRouteRepository deliveryRouteRepository)
        {
            _repository = deliveryRouteRepository ?? throw new ArgumentNullException(nameof(deliveryRouteRepository));
        }
        public async Task<GetRouteByIdQueryResponse> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
        {
            var route =  await _repository.GetByIdAsync(RouteId.From(request.Id), cancellationToken);

            if (route is null)
                throw new KeyNotFoundException("Route not found");

            return new GetRouteByIdQueryResponse(DeliveryRouteDto.From(route));
        }
    }
}

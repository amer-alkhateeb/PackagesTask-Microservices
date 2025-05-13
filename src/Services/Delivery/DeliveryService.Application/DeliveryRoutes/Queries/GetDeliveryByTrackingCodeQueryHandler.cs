using DeliveryService.Application.CQRS;
using DeliveryService.Application.Dtos;
using DeliveryService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.DeliveryRoutes.Queries
{
    public sealed class GetDeliveryByTrackingCodeQueryHandler : IQueryHandler<GetDeliveryByTrackingCodeQuery, GetDeliveryByTrackingCodeQueryResponse>

    {
        private readonly IDeliveryRouteRepository _repository;

        public GetDeliveryByTrackingCodeQueryHandler(IDeliveryRouteRepository deliveryRouteRepository)
        {
            _repository = deliveryRouteRepository ?? throw new ArgumentNullException(nameof(deliveryRouteRepository));
        }

        public async Task<GetDeliveryByTrackingCodeQueryResponse> Handle(GetDeliveryByTrackingCodeQuery request, CancellationToken cancellationToken)
        {
            var delivery = await _repository.GetByTrackingCodeAsync(request.TrackingCode, cancellationToken);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery not found");

            return new GetDeliveryByTrackingCodeQueryResponse( DeliveryDetailsDto.From(delivery));
        }
    }
}

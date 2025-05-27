using DeliveryService.API.Contracts.DTOs;
using DeliveryService.Application.DeliveryRoutes.Commands;
using DeliveryService.Application.DeliveryRoutes.Queries;
using DeliveryService.Application.Dtos;
using Mapster;

namespace DeliveryService.API.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateRouteRequest, ScheduleDeliveryRouteCommand>.NewConfig();

            TypeAdapterConfig<DeliveryRouteDto, RouteResponse>
                .NewConfig()
                  .Map(dest => dest.Deliveries, src => src.Deliveries.Adapt<List<DeliverySummary>>());

            TypeAdapterConfig<DeliveryDto, DeliverySummary>.NewConfig();
            TypeAdapterConfig<DeliveryDetailsDto, DeliveryResponse>.NewConfig();
            TypeAdapterConfig<GetDeliveryByTrackingCodeQueryResponse, DeliveryResponse>.NewConfig();
        }
    }
}

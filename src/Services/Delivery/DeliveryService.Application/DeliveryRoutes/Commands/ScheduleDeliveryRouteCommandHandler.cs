using DeliveryService.Application.CQRS;
using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;

namespace DeliveryService.Application.DeliveryRoutes.Commands
{
    public sealed class ScheduleDeliveryRouteCommandHandler : ICommandHandler<ScheduleDeliveryRouteCommand, ScheduleDeliveryRouteCommandResponse>
    {
        private readonly IDeliveryRouteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleDeliveryRouteCommandHandler(IDeliveryRouteRepository deliveryRouteRepository, IUnitOfWork unitOfWork)
        {
            _repository = deliveryRouteRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ScheduleDeliveryRouteCommandResponse> Handle(ScheduleDeliveryRouteCommand request, CancellationToken cancellationToken)
        {
            var route = DeliveryRoute.Schedule(
                      RouteId.Create(),
                      TruckId.From(request.TruckId),
                      DriverId.From(request.DriverId),
                      request.ScheduledDate
                  );

            foreach (var packageId in request.PackageIds)
            {
                var delivery = Delivery.Schedule(packageId, request.ScheduledDate.AddHours(2));
                route.AddDelivery(delivery);
            }

            await _repository.AddAsync(route, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ScheduleDeliveryRouteCommandResponse(route.Id.Value);
        }
    }
}

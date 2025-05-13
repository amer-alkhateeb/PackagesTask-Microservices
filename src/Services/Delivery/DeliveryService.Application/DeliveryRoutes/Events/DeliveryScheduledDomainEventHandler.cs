using DeliveryService.Domain.Events;
using MassTransit;
using MediatR;
using Shared.Contracts.Events;

namespace DeliveryService.Application.DeliveryRoutes.Events
{
    public class DeliveryScheduledDomainEventHandler : INotificationHandler<DeliveryScheduledDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public DeliveryScheduledDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DeliveryScheduledDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new DeliveryScheduledIntegrationEvent(
                notification.RouteId.Value,
                notification.DeliveryId.Value,
                notification.PackageId,
                notification.EstimatedTime
            );

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
using DeliveryService.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Infrastructure.Messaging
{
    public class MassTransitIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitIntegrationEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
            => _publishEndpoint.Publish(@event, cancellationToken);
    }
}

using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace PackagesService.Application.Messaging
{
    public class DeliveryScheduledConsumer : IConsumer<DeliveryScheduledIntegrationEvent>
    {
        private readonly ILogger<DeliveryScheduledConsumer> _logger;
        // Inject your use case or repository if needed

        public DeliveryScheduledConsumer(ILogger<DeliveryScheduledConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DeliveryScheduledIntegrationEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("Received DeliveryScheduled event: {PackageId}", message.PackageId);

            // Optional: handle package tracking creation or updates here
            // await _useCase.TrackPackageAsync(message.PackageId, ...);
        }
    }
}

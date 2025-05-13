namespace DeliveryService.Application.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
    }
}

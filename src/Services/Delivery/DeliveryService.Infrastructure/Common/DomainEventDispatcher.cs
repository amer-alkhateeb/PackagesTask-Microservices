using DeliveryService.Domain.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Common
{
    public static class DomainEventDispatcher
    
    {
        public static async Task DispatchEventsAsync(DbContext context, IMediator mediator)
        {
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = aggregates
                .SelectMany(a => a.ClearDomainEvents())
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}

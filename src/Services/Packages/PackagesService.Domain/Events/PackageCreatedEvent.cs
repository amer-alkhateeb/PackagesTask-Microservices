using PackagesService.Domain.Abstractions;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Domain.Events
{
    public sealed record PackageCreatedEvent(PackageId PackageId) : IDomainEvent
    {
    }
}

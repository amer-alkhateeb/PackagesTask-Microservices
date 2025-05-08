using PackagesService.Domain.Abstractions;
using PackagesService.Domain.Events;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Domain.Models
{
    public sealed class Package : Aggregate<PackageId>
    {
        public string Sender { get; private set; } = default!;
        public string Recipient { get;private set; } = default!;
        public Weight Weight { get;private set; } = default!;
        public Address Destination { get; private set; } = default!;

        private Package()
        {
        }
        private Package(PackageId id, string sender, string recipient, Weight weight, Address destination)
        {
            Sender = sender;
            Recipient = recipient;
            Weight = weight;
            Destination = destination;

            AddDomainEvent(new PackageCreatedEvent(id));
        }

        public static Package Of(string sender, string recipient, Weight weight, Address destination)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sender);
            ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
            ArgumentException.ThrowIfNullOrWhiteSpace(weight.ToString());
            ArgumentException.ThrowIfNullOrWhiteSpace(destination.ToString());
            var id = PackageId.Create();
            var order = new Package(id, sender, recipient, weight, destination);

            order.AddDomainEvent(new PackageCreatedEvent(id));
            return order;
        }
    }
}

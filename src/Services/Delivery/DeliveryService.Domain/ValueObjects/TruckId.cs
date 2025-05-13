namespace DeliveryService.Domain.ValueObjects
{
    public sealed record TruckId(Guid Value)
    {
        public static TruckId Create() => new(Guid.NewGuid());
        public static TruckId From (Guid Value) =>new (Value);
        public override string ToString() => Value.ToString();
    }
}

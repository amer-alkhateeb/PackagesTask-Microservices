namespace DeliveryService.Domain.ValueObjects
{
    public sealed record DeliveryId (Guid Value)
    {
        public static DeliveryId Create() => new(Guid.NewGuid());
        public static DeliveryId From (Guid Value) => new(Value);
        public override string ToString() => Value.ToString();
    }
}

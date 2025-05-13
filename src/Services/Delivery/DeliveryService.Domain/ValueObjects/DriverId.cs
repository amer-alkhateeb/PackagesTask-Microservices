namespace DeliveryService.Domain.ValueObjects
{
    public sealed record DriverId (Guid Value)
    {
        public static DriverId Create() => new(Guid.NewGuid());
        public static DriverId From (Guid Value)=>new(Value);
        public override string ToString() => Value.ToString();
    }
}

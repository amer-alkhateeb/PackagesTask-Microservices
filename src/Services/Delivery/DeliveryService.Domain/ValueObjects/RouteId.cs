namespace DeliveryService.Domain.ValueObjects
{
    public sealed record RouteId(Guid Value)
    {
        public static RouteId Create() => new(Guid.NewGuid());
        public static RouteId From(Guid value) => new(value);
        public override string ToString() => Value.ToString();
    }
}

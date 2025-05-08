namespace PackagesService.Domain.ValueObjects
{
    public sealed record PackageId
    {
        public Guid Value { get; }
        private PackageId(Guid value) => Value = value;
        public static PackageId Create() => new(Guid.NewGuid());

        public static PackageId From(Guid value) => new(value);
        public override string ToString() => Value.ToString();
    }
}

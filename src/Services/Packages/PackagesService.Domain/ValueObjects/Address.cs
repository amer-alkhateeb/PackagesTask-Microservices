namespace PackagesService.Domain.ValueObjects
{
    public sealed record  Address
    {
        public string Street { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string ZIP { get; private set; } = default!;
        private Address() { }
        private Address(string street, string city, string zip)
        {
            Street = street;
            City = city;
            ZIP = zip;
        }
        public static Address Of(string street, string city, string zip)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(street);
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(zip);
            return new Address(street, city, zip);
        }
    }
}
    

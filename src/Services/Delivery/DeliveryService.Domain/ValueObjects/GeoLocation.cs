namespace DeliveryService.Domain.ValueObjects
{
    public sealed record GeoLocation
    {
        public double Latitude { get; private set; } = default!;
        public double Longitude { get; private set; } = default!;

        private GeoLocation() { }
        public GeoLocation (double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;    
        }

        public static GeoLocation Of (double Latitude , double Longitude)
        {
            return new GeoLocation(Latitude, Longitude);
        }

    }
}

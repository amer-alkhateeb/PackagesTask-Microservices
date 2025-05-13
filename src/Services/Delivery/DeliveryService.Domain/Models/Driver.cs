using DeliveryService.Domain.Abstraction;
using DeliveryService.Domain.ValueObjects;

namespace DeliveryService.Domain.Models
{
    public sealed class Driver : Entity<DriverId>
    {
        public string FullName { get;private set; }
        public string Phone { get;private set; }
        public GeoLocation CurrentLocation { get;private set; }

        private Driver() { }

        private Driver(DriverId id , string fullName , string phone , GeoLocation location) {
            FullName= fullName;
            Phone= phone;
            CurrentLocation = location;  
        }
        public static Driver Hire(string fullName, string phone, GeoLocation location)
        {
            return new Driver(DriverId.Create(), fullName, phone, location);
        }

        public void UpdateLocation(GeoLocation location)
        {

            CurrentLocation = location;

        }
    }
}

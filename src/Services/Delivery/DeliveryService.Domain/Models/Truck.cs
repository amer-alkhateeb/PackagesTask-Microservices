using DeliveryService.Domain.Abstraction;
using DeliveryService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Models
{
    public sealed class Truck : Entity<TruckId>
    {
        public string RegistrationNumber { get;private set; }
        public decimal CapacityKG { get;private set; }
        public DateTime AvailableFrom { get;private set; }

        private Truck() { }
        private Truck (TruckId id , string registrationNumber, decimal capacityKG , DateTime availableFrom)
        {
            Id = id;
            RegistrationNumber = registrationNumber;
            CapacityKG = capacityKG;
            AvailableFrom = availableFrom;
        }
        public static Truck Register(string regNumber, decimal capacityKg)
        {
            return new Truck(TruckId.Create(), regNumber, capacityKg , DateTime.UtcNow);
        }

        public void SetNextAvailable(DateTime time)
        {
            AvailableFrom = time;
        }
    }
}

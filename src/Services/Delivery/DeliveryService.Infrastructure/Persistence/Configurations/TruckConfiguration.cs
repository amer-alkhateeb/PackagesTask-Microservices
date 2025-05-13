using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.Persistence.Configurations
{
    public class TruckConfiguration: IEntityTypeConfiguration<Truck>
    {
        public void Configure(EntityTypeBuilder<Truck> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, val => TruckId.From(val));

            builder.Property(x => x.RegistrationNumber).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CapacityKG).IsRequired();
            builder.Property(x => x.AvailableFrom);
        }
    }
}

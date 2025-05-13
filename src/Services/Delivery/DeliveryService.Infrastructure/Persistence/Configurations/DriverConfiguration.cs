using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.Persistence.Configurations
{
    internal class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, val => DriverId.From(val));
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);

            builder.OwnsOne(d => d.CurrentLocation, geo =>
            {
                geo.Property(g => g.Latitude).HasColumnName("Latitude");
                geo.Property(g => g.Longitude).HasColumnName("Longitude");
            });
        }
    }
}

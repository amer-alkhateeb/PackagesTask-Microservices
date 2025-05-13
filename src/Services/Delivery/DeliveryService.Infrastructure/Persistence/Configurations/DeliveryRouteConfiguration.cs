using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Infrastructure.Persistence.Configurations
{
    public class DeliveryRouteConfiguration : IEntityTypeConfiguration<DeliveryRoute>
    {
        public void Configure(EntityTypeBuilder<DeliveryRoute> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, val => RouteId.From(val));

            builder.Property(x => x.TruckId)
                .HasConversion(id => id.Value, val => TruckId.From(val));

            builder.Property(x => x.DriverId)
                .HasConversion(id => id.Value, val => DriverId.From(val));

            builder.Property(x => x.ScheduleDate);

            builder.OwnsMany(x => x.Deliveries, b =>
            {
                b.WithOwner().HasForeignKey("RouteId");

                b.HasKey(x => x.Id);

                b.Property(x => x.Id)
                    .HasConversion(
                        id => id.Value,
                        value => DeliveryId.From(value));

                b.Property(x => x.PackageId).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.EstimatedTime);
                b.Property(x => x.ActualTime);
                b.Property(x => x.TrackingCode);
            });
        }
    }
}

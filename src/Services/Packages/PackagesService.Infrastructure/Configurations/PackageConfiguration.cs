using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Infrastructure.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(p=> p.Id);
            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => PackageId.From(value));

            builder.Property(p => p.Sender)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Recipient)
                .IsRequired()
                .HasMaxLength(200);

            builder.ComplexProperty(p => p.Weight, weightBuilder =>
            {
                weightBuilder.Property(a => a.Kilograms).IsRequired();
            });

            builder.ComplexProperty(p => p.Destination, destinationBuilder =>
            {
                destinationBuilder.Property(a => a.City).IsRequired().HasMaxLength(200);
                destinationBuilder.Property(a => a.Street).IsRequired().HasMaxLength(200);
                destinationBuilder.Property(a => a.ZIP).IsRequired().HasMaxLength(50);
            });
        }
    }
}

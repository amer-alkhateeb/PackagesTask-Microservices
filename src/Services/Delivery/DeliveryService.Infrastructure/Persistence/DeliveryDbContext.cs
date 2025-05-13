using DeliveryService.Domain.Models;
using DeliveryService.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Persistence
{
    public class DeliveryDbContext : DbContext
    {
        private readonly IMediator _mediator;
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options, IMediator? mediator = null) : base(options)
        {
            _mediator = mediator;
        }
        public DbSet<Delivery> Deliveries => Set<Delivery>();
        public DbSet<DeliveryRoute> DeliveryRoutes => Set<DeliveryRoute>();
        public DbSet<Truck> Trucks => Set<Truck>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DomainEventDispatcher.DispatchEventsAsync(this, _mediator);
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);
        }
    }
}

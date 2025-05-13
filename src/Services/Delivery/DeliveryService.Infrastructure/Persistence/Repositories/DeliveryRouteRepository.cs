using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Infrastructure.Persistence.Repositories
{
    public class DeliveryRouteRepository : Repository<DeliveryRoute, RouteId>, IDeliveryRouteRepository
    {
        private readonly DeliveryDbContext _context;

        public DeliveryRouteRepository(DeliveryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Delivery?> GetByTrackingCodeAsync(string trackingCode, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .FirstOrDefaultAsync(d => d.TrackingCode == trackingCode, cancellationToken);
        }
        public async Task AddAsync(DeliveryRoute entity, CancellationToken cancellationToken)
        {
            await _context.DeliveryRoutes.AddAsync(entity, cancellationToken);
        }

        public async Task<DeliveryRoute?> GetByIdAsync(RouteId id, CancellationToken cancellationToken)
        {
            return await _context.DeliveryRoutes
                .Include(r => r.Deliveries)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}

using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Persistence.Repositories
{
    public class Repository<T, TId> : IRepository<T, TId>
    where T : Entity<TId>
    {
        protected readonly DeliveryDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DeliveryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id!.Equals(id) && !e.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T item, CancellationToken cancellationToken = default)
        {
            var entry = await _dbSet.AddAsync(item, cancellationToken);
            return entry.Entity;
        }

        public async Task<T> UpdateAsync(TId id, T item, CancellationToken cancellationToken = default)
        {
            var existing = await _dbSet.FindAsync([id], cancellationToken);
            if (existing == null || existing.IsDeleted)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found or deleted.");
            }

            _context.Entry(existing).CurrentValues.SetValues(item);
            return existing;
        }

        public async Task<T?> DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync([id], cancellationToken);
            if (entity == null || entity.IsDeleted)
            {
                return null;
            }

            entity.SoftDelete(); // Mark as deleted
            _dbSet.Update(entity); // EF tracks soft-deleted state
            return entity;
        }
    }
}


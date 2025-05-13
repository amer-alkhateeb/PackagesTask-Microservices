using PackagesService.Application.Interfaces;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Infrastructure.Persistence
{
    public class PackagesRepository : IPackageRepository
    {
        private readonly PackageDbContext _dbContext;
        public PackagesRepository(PackageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Package package, CancellationToken cancellationToken)
        {
          await _dbContext.AddAsync(package, cancellationToken);
          await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Package?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return  await _dbContext.packages.FindAsync(id, cancellationToken);
        }
    }
}

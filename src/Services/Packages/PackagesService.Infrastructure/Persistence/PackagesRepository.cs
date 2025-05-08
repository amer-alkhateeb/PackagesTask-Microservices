using PackagesService.Application.Interfaces;
using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesTask.Infrastructure.Persistence
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
          await _dbContext.AddAsync(package);
          await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Package?> GetByIdAsync(PackageId id, CancellationToken cancellationToken)
        {
            return  await _dbContext.packages.FindAsync(id, cancellationToken);
        }
    }
}

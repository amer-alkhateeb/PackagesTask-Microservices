using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.Application.Interfaces
{
    public interface IPackageRepository
    {
        Task AddAsync(Package package,CancellationToken cancellationToken);
        Task <Package?> GetByIdAsync (Guid id ,CancellationToken cancellationToken);
    }
}

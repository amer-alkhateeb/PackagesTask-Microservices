using PackagesService.Domain.Models;
using PackagesService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesService.Application.Interfaces
{
    public interface IPackageRepository
    {
        Task AddAsync(Package package,CancellationToken cancellationToken);
        Task <Package?> GetByIdAsync (PackageId id ,CancellationToken cancellationToken);
    }
}

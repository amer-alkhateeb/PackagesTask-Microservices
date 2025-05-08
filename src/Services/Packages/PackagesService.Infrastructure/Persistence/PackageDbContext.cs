using Microsoft.EntityFrameworkCore;
using PackagesService.Domain.Models;

namespace PackagesTask.Infrastructure.Persistence
{
    public class PackageDbContext : DbContext
    {
        public PackageDbContext() { }
        public PackageDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Package> packages => Set<Package>();   

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PackageDbContext).Assembly);
        }
    }
}

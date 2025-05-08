using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PackagesTask.Infrastructure.Persistence;

namespace PackagesService.Infrastructure.Persistence
{
    public class DesignTimePackageDbContextFactory : IDesignTimeDbContextFactory<PackageDbContext>
    {
        public PackageDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PackageDbContext>();
            var connectionString = configuration.GetConnectionString("Postgres");

            optionsBuilder.UseNpgsql(connectionString);

            return new PackageDbContext(optionsBuilder.Options);
        }
    }
}

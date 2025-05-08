using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.Application.Interfaces;
using PackagesTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace PackagesTask.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Postgres");

            services.AddDbContext<PackageDbContext>(options =>
            options.UseNpgsql(connectionString));

            services.AddScoped<IPackageRepository, PackagesRepository>();

            return services;
        }
    }
}

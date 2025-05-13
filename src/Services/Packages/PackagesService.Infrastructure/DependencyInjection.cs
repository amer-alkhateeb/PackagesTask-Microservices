using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.Application.Interfaces;
using PackagesService.Application.Messaging;
using PackagesService.Infrastructure.Persistence;
namespace PackagesService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Postgres");

            services.AddDbContext<PackageDbContext>(options =>
            options.UseNpgsql(connectionString));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<DeliveryScheduledConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("package-service-queue", e =>
                    {
                        e.ConfigureConsumer<DeliveryScheduledConsumer>(context);
                    });
                });
            });

            services.AddScoped<IPackageRepository, PackagesRepository>();

            return services;
        }
    }
}

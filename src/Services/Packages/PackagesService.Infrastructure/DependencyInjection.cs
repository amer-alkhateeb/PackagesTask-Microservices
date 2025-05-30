using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.Application.Interfaces;
using PackagesService.Application.Messaging;
using PackagesService.Infrastructure.Persistence;
using RabbitMQ.Client;
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

            services.AddHealthChecks()
                .AddNpgSql(config.GetConnectionString("Postgres"),
                    name: "postgresql",
                    timeout: TimeSpan.FromSeconds(5))
               .AddRabbitMQ(sp =>
               {
                   var factory = new ConnectionFactory
                   {
                       Uri = new Uri("amqp://guest:guest@rabbitmq")
                   };
                   return factory.CreateConnectionAsync();
               }, name: "RabbitMQ");

            return services;
        }
    }
}

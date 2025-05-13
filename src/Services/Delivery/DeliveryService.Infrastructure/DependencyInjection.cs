using DeliveryService.Application.Interfaces;
using DeliveryService.Infrastructure.Messaging;
using DeliveryService.Infrastructure.Persistence;
using DeliveryService.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DeliveryDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("SqlServer")));


            // Add MassTransit with RabbitMQ
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            // Register publisher abstraction


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDeliveryRouteRepository, DeliveryRouteRepository>();
            services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();
            return services;
        }
    }
}

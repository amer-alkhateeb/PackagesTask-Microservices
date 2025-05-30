using DeliveryService.Application.Interfaces;
using DeliveryService.Infrastructure.Messaging;
using DeliveryService.Infrastructure.Persistence;
using DeliveryService.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

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



            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDeliveryRouteRepository, DeliveryRouteRepository>();
            services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();

            services.AddHealthChecks()
                .AddSqlServer(config.GetConnectionString("SqlServer"), name: "SQL Server")
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

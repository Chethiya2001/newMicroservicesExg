using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit;

public static class Extentions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
    {

        // MassTransit-RabbitMQ Config
        services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly()); //add consumers
                configure.UsingRabbitMq((context, cfg) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                    cfg.Host(rabbitMqSettings!.Host);
                    //how the ques are create in rbmq
                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings!.ServiceName, false));

                });
            }
        );

        return services;
    }

}

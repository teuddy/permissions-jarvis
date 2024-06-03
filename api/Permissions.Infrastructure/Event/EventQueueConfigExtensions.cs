using Confluent.Kafka;
using KafkaFlow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Permissions.Domain;
using Permissions.Domain.Dto;
using Permissions.Domain.Events;
using Permissions.Infrastructure.EventQueue.Handlers;

namespace Permissions.Infrastructure.Event;

public static class EventQueueConfigExtensions
{
    public static void AddEventQueue(this IServiceCollection services, IConfiguration configuration)
    {
        var brokers = configuration["Kafka:Brokers"] ?? "localhost:9092";
        var topic = configuration["Kafka:Topic"] ?? string.Empty;
        
        services.AddKafka(
            kafka => kafka
                .UseMicrosoftLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(
                            brokers.Split(',')
                        )
                        .CreateTopicIfNotExists(topic, 1,1)
                        .AddProducer(Constants.ProducerName, 
                            producer => producer
                                                    .DefaultTopic(topic)
                                                    .WithCompression(CompressionType.Gzip))
                ));

        services.AddTransient(typeof(IPublisher<PermissionEvent>), typeof(PermissionEventPublisher));
    }
}
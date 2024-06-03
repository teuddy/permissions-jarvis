using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;
using Permissions.Domain.Events;
using Permissions.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Permissions.Domain.Dto;
using Constants = Permissions.Domain.Constants;

namespace Permissions.Infrastructure.EventQueue.Handlers;

public class PermissionEventPublisher : IPublisher<PermissionEvent>
{
    private readonly IMessageProducer _producer;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PermissionEventPublisher> _logger;
    
    public PermissionEventPublisher(IProducerAccessor producerAccessor, IConfiguration configuration, ILogger<PermissionEventPublisher> logger)
    {
        _producer = producerAccessor.GetProducer(Constants.ProducerName) ?? throw new ArgumentNullException(nameof(producerAccessor));
        
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task PublishMessageAsync(PermissionEvent message)
    {
        var topic = _configuration["Kafka:Topic"] ?? "";

        var messageKey = Guid.NewGuid();
        
        var serialized = JsonSerializer.Serialize(message);
        
        var messageBytes = Encoding.UTF8.GetBytes(serialized);
        
        var status = await _producer.ProduceAsync(
            topic,
            messageKey,
            messageBytes
        );
        
        if (status.Status == PersistenceStatus.Persisted)
        {
            _logger.LogInformation("Message with key {MessageKey} was produced successfully", messageKey);
        }
        else
        {
            _logger.LogError("Message with key {MessageKey} was not produced, current status: {StatusStatus}", messageKey, status.Status);
        }
    }
}
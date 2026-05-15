using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Services
{
    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishMessageAsync<T>(string topic, T message)
        {
            try
            {
                var messageJson = JsonSerializer.Serialize(message);
                var kafkaMessage = new Message<Null, string> { Value = messageJson };
                
                var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);
                _logger.LogInformation($"Message delivered to {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
    }
}

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OrderConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Set up logging
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information);
            });
            var logger = loggerFactory.CreateLogger<Program>();

            var kafkaConfig = configuration.GetSection("Kafka");
            var bootstrapServers = kafkaConfig["BootstrapServers"];
            var topic = kafkaConfig["Topic"] ?? "orders";
            var groupId = kafkaConfig["GroupId"] ?? "order-consumer-group";

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            logger.LogInformation("Starting Order Consumer...");
            logger.LogInformation($"Connecting to {bootstrapServers}, Topic: {topic}, GroupId: {groupId}");

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cts.Token);
                        
                        logger.LogInformation($"Received message at {consumeResult.TopicPartitionOffset}");
                        
                        try 
                        {
                            var order = JsonSerializer.Deserialize<OrderDTO>(consumeResult.Message.Value, new JsonSerializerOptions 
                            { 
                                PropertyNameCaseInsensitive = true 
                            });

                            if (order != null)
                            {
                                logger.LogInformation("----------------------------------------");
                                logger.LogInformation($"Order ID: {order.Id}");
                                logger.LogInformation($"User ID:  {order.UserId}");
                                logger.LogInformation($"Date:     {order.OrderDate}");
                                logger.LogInformation($"Sum:      ${order.OrderSum}");
                                logger.LogInformation($"Items:    {order.OrderItems?.Count ?? 0}");
                                logger.LogInformation("----------------------------------------");
                            }
                        }
                        catch (JsonException ex)
                        {
                            logger.LogWarning($"Failed to deserialize message: {ex.Message}");
                            logger.LogDebug($"Raw message: {consumeResult.Message.Value}");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        logger.LogError($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Closing consumer...");
            }
            finally
            {
                consumer.Close();
                logger.LogInformation("Consumer stopped.");
            }
        }
    }
}

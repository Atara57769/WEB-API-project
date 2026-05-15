namespace Services
{
    public interface IKafkaProducerService
    {
        Task PublishMessageAsync<T>(string topic, T message);
    }
}

using Services;

namespace ServiceContracts
{
    public interface IKafkaService
    {
        Task<List<ProducerKafka>> ProduceMessages();

        Task ConsumeMessages(KafkaConfig consumerConfig, string Topic , string consumerName);
    }
}

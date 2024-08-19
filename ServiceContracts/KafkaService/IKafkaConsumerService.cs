

namespace ServiceContracts.KafkaService
{
    public interface IKafkaConsumerService
    {
        Task ConsumeMessages(KafkaConfig consumerConfig, string Topic, string consumerName);
    }
}

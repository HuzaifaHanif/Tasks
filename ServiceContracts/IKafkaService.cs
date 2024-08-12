using Services;

namespace ServiceContracts
{
    public interface IKafkaService
    {
        Task<List<ProducerKafka>> ProduceMessages();
    }
}

using ServiceContracts.AzureBusService;
using ServiceContracts.KafkaService;
using ServiceContracts.RabbitMQService;

namespace Services
{
    public static class CustomDelegates
    {
        public delegate Task LoggingAzureBusServiceMessages(ConsumerAzureBus consumerObj);

        public delegate Task LoggingKafkaServiceMessages(ConsumerKafka consumerObj);

        public delegate Task LoggingRabbitMQServiceMessages(ConsumerRabbitMq consumerObj);
    }
}

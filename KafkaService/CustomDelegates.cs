
using ServiceContracts;

namespace Services
{
    public static class CustomDelegates
    {
        public delegate Task LoggingAzueBusServiceMessages(ConsumerAzureBus consumerObj , string connectionstring);

        public delegate Task LoggingKafkaServiceMessages(ConsumerKafka consumerObj, string connectionstring);

        public delegate Task LoggingRabbitMQServiceMessages(ConsumerRabbitMq consumerObj, string connectionstring);
    }
}

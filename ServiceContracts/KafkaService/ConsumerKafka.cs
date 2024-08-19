

namespace ServiceContracts.KafkaService
{
    public class ConsumerKafka
    {
        public Guid Id { get; set; }

        public string Topic { get; set; }

        public int Partition { get; set; }

        public string Message { get; set; }

        public string ConsumerName { get; set; }

    }
}

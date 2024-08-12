using Confluent.Kafka;
using ServiceContracts;
using Services;

namespace Services
{
    public class KafkaService : IKafkaService
    {
        public async Task<List<ProducerKafka>> ProduceMessages()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",

            };

            List<ProducerKafka> results = new List<ProducerKafka>();

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                for (int i = 0; i < 10; i++)
                {
                    Guid id = Guid.NewGuid();
                    var result = await producer.ProduceAsync("KafkaPublisher", new Message<Null, string>
                    {
                        Value = $"message from id {id}"
                    });

                    Console.WriteLine($"produced message to topic {result.Topic}, partition {result.Partition}");

                    ProducerKafka resultobj = new ProducerKafka
                    {
                        Topic = result.Topic,
                        Partition = result.Partition,
                        Message = result.Value,
                        Timestamp = result.Timestamp.UtcDateTime,
                        Guid = id
                    };

                    results.Add(resultobj);
                }
            }

            return results;
        }

    }
}

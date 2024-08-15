using Confluent.Kafka;
using ServiceContracts;
using Services;

namespace Services
{
    public class KafkaService : IKafkaService
    {
        private readonly CustomDelegates.LoggingKafkaServiceMessages _dbDelegate;

        public KafkaService(CustomDelegates.LoggingKafkaServiceMessages dbDelegate)
        {
            _dbDelegate = dbDelegate;
        }

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

        public async Task ConsumeMessages(KafkaConfig consumerConfig, string topic, string consumerName)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = consumerConfig.BootstrapServers,
                GroupId = consumerName
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);

                var cancellationTokenSource = new CancellationTokenSource();

                while (true)
                {
                    var consumeResult = consumer.Consume(cancellationTokenSource.Token);

                    if (consumeResult != null && !string.IsNullOrEmpty(consumeResult.Message.Value))
                    {
                        string[] messages = consumeResult.Message.Value.Split(' ');
                        ConsumerKafka consumedMessage = new ConsumerKafka();
                        consumedMessage.Id = Guid.Parse(messages[3]);
                        consumedMessage.Message = consumeResult.Message.Value;
                        consumedMessage.Topic = consumeResult.Topic;
                        consumedMessage.Partition = consumeResult.Partition;
                        consumedMessage.ConsumerName = consumerName;

                        await _dbDelegate.Invoke(consumedMessage , "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                    }
                }

            }
        }

    }
}

using Confluent.Kafka;
using ServiceContracts.KafkaService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.KafkaService
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly CustomDelegates.LoggingKafkaServiceMessages _dbDelegate;

        public KafkaConsumerService(CustomDelegates.LoggingKafkaServiceMessages dbDelegate)
        {
            _dbDelegate = dbDelegate;
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

                        await _dbDelegate.Invoke(consumedMessage);

                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                    }
                }

            }
        }
    }
}

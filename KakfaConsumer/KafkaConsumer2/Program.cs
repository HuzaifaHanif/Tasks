// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using KafkaConsumer1;
using KafkaConsumer2;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("\tKafka\t.............Consumer 2.........");
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "huzaifa2",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe("KafkaPublisher");

            var cancellationTokenSource = new CancellationTokenSource();

            while (true)
            {
                var consumeResult = consumer.Consume(cancellationTokenSource.Token);

                if (consumeResult != null && !string.IsNullOrEmpty(consumeResult.Message.Value))
                {
                    string[] messages = consumeResult.Message.Value.Split(' ');

                    Kafka consumedMessage = new Kafka();
                    consumedMessage.Id = Guid.Parse(messages[3]);
                    consumedMessage.Message = consumeResult.Message.Value;
                    consumedMessage.Topic = consumeResult.Topic;
                    consumedMessage.Partition = consumeResult.Partition;
                    consumedMessage.ConsumerName = "Consumer2";

                    DatabaseSevice db = DatabaseSevice.GetInstance();
                    await db.LogDb(consumedMessage);
                    Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                   
                }
            }

            consumer.Close();
        }

    }
}


//produced message to topic KafkaPublisher, partition [0]
//Consumed message 'message from id 9364f2f2-fbc7-4651-828b-c47a782fc588' at: 'KafkaPublisher [[0]] @90'.
//message from id 9364f2f2-fbc7-4651-828b-c47a782fc588
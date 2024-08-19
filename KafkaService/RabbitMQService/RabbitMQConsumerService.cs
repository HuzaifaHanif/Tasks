using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceContracts;
using ServiceContracts.RabbitMQService;
using System.Text;

namespace Services.RabbitMQService
{
    public class RabbitMQConsumerService : IRabbitMQConsumerService
    {
        private readonly CustomDelegates.LoggingRabbitMQServiceMessages _logDb;

        public RabbitMQConsumerService(CustomDelegates.LoggingRabbitMQServiceMessages logDB)
        {
            _logDb = logDB;
        }

        public async Task ConsumeMessages(string Url, string queueName, string exchange, string consumerName)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(Url)
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            ConsumerRabbitMq recievedMessage = new ConsumerRabbitMq();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JObject.Parse(Encoding.UTF8.GetString(body));

                recievedMessage.Guid = Guid.Parse(message["id"].ToString());
                recievedMessage.Message = message["message"].ToString();
                recievedMessage.Queue = queueName;
                recievedMessage.Exchange = exchange;
                recievedMessage.ConsumerName = consumerName;

                await _logDb.Invoke(recievedMessage);

                Console.WriteLine($"Received message: {message}");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");

            Console.ReadLine();

        }
    }

}

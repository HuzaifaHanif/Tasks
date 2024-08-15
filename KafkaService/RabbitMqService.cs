using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceContracts;
using System.Text;

namespace Services
{
    public class RabbitMqService : IRabbitMQService
    {
        private readonly CustomDelegates.LoggingRabbitMQServiceMessages _logDb;

        public RabbitMqService(CustomDelegates.LoggingRabbitMQServiceMessages logDB)
        {
            _logDb = logDB;
        }

        public List<ProducerRabbitMQ> ProduceMessages()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://user:password@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "Huzaifa", type: ExchangeType.Fanout);

            List<string> queues = new List<string>
            {
                "queue1" , "queue2" , "queue3"
            };

            foreach (var queueName in queues)
            {
                channel.QueueDeclare(queue: queueName,
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

                channel.QueueBind(
                    queue: queueName,
                    exchange: "Huzaifa",
                    routingKey: "1037"
                    );
            }


            List<ProducerRabbitMQ> messages = new List<ProducerRabbitMQ>();

            for (int i = 0; i < 10; i++)
            {
                ProducerRabbitMQ publishedMessage = new ProducerRabbitMQ();

                var message = new { id = Guid.NewGuid(), name = "Producer", message = "hello i am Huzaifa" };

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(
                    exchange: "Huzaifa",
                    routingKey: "1037",
                    basicProperties: null,
                    body: body);

                publishedMessage.Guid = message.id;
                publishedMessage.Queue = channel.CurrentQueue;
                publishedMessage.Message = message.message;
                publishedMessage.Timestamp = DateTime.UtcNow;
                messages.Add( publishedMessage );     
            }

            return messages;
        }

        public async Task ConsumeMessages(string Url, string queueName, string exchange , string consumerName)
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

                await _logDb.Invoke(recievedMessage , "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

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

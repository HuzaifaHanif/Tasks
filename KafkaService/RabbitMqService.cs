using Newtonsoft.Json;
using RabbitMQ.Client;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RabbitMqService : IRabbitMQService
    {
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
    }
}

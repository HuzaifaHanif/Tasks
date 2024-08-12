// See https://aka.ms/new-console-template for more information
using KafkaConsumer2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceContracts;
using System.Text;
using System.Text.Json.Serialization;

static class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://user:password@localhost:5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "queue2",
                 durable: true,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);

        //channel.QueueBind("queue2", "Huzaifa", "1037");

        Console.WriteLine("\t\t\tRabbitMQConsumer 2\n\n Waiting For Meassges...\n\n");

        RabbitMq recievedMessage = new RabbitMq();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JObject.Parse(Encoding.UTF8.GetString(body));

            recievedMessage.Guid = Guid.Parse(message["id"].ToString());
            recievedMessage.Message = message["message"].ToString();
            recievedMessage.Queue = "queue2";
            recievedMessage.Exchange = ea.Exchange;
            recievedMessage.ConsumerName = "consumer 2";

            DatabaseSevice db = DatabaseSevice.GetInstance();
            await db.LogDb(recievedMessage);

            Console.WriteLine($"Received message: {message}");
        };
        channel.BasicConsume(queue: "queue2",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");

        Console.ReadLine();

    }
}

//{"id":"81d573c4-9627-47d5-92a4-4c89cb0f127e","name":"Producer","message":"hello i am Huzaifa"}
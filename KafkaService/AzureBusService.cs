using Azure.Messaging.ServiceBus;
using ServiceContracts;

namespace Services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly CustomDelegates.LoggingAzueBusServiceMessages _logDbDelegate;

        public AzureBusService(CustomDelegates.LoggingAzueBusServiceMessages logDbDelegate)
        {
            _logDbDelegate = logDbDelegate;
        }

        public async Task<List<ProducerAzureBus>> ProduceMessages(string connectionstring , string topicName)
        {
            ServiceBusClient client;
            ServiceBusSender sender;

            client = new ServiceBusClient(connectionstring);
            sender = client.CreateSender(topicName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            List<ProducerAzureBus> messages = new List<ProducerAzureBus>();

            for (int i = 0; i < 5; i++)
            {
                ProducerAzureBus message = new ProducerAzureBus();
                message.Topic = topicName;
                message.Guid = Guid.NewGuid();
                message.Message = $"Message with id : {message.Guid}";

                if (!messageBatch.TryAddMessage(new ServiceBusMessage(message.Message)))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }

                messages.Add(message);
            }

            await sender.SendMessagesAsync(messageBatch);
            return messages;

        }

        public async Task ConsumeMessages(string connectionstring, string topic, string subscriberName)
        {
            ServiceBusClient client;
            ServiceBusProcessor processor;

            client = new ServiceBusClient(connectionstring);
            processor = client.CreateProcessor(topic, subscriberName, new ServiceBusProcessorOptions());

            ConsumerAzureBus producedMessage = new ConsumerAzureBus();

            processor.ProcessMessageAsync += async args =>
            {
                var message = args.Message.Body.ToString();

                producedMessage.Topic = topic;
                producedMessage.Message = message.ToString();
                string[] msg = message.ToString().Split(':');
                Guid id = Guid.Parse(msg[1]);
                producedMessage.Guid = id;
                producedMessage.ConsumerName = subscriberName;

                await _logDbDelegate.Invoke(producedMessage, "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                string body = args.Message.Body.ToString();
                Console.WriteLine($"Received: {body} from subscription.");

                await args.CompleteMessageAsync(args.Message);
            };

            processor.ProcessErrorAsync += args =>
            {
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync();

            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
        }
    }
}

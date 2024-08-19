using Azure.Messaging.ServiceBus;
using ServiceContracts.AzureBusService;

namespace Services.AzureBusService
{
    public class AzureBusConsumerService : IAzureBusConsumerService
    {
        private readonly CustomDelegates.LoggingAzureBusServiceMessages _logDbDelegate;

        public AzureBusConsumerService(CustomDelegates.LoggingAzureBusServiceMessages logDbDelegate)
        {
            _logDbDelegate = logDbDelegate;
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

                await _logDbDelegate.Invoke(producedMessage);

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

using Azure.Messaging.ServiceBus;
using AzureBusServiceConsumer1;
using Newtonsoft.Json.Linq;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("\tAzure Bus Service\t.............Consumer 1.........");

        ServiceBusClient client;
        ServiceBusProcessor processor;

        client = new ServiceBusClient("Endpoint=sb://huzaifaservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XawyVwykXqUjUI79C3RVFzzgc47imiqTa+ASbH+o4TA=");
        processor = client.CreateProcessor("onboardingtask", "subscriber1", new ServiceBusProcessorOptions());

        AzureBus producedMessage = new AzureBus();

        processor.ProcessMessageAsync += async args =>
        {
            var message = args.Message.Body.ToString();

            producedMessage.Topic = "onboardingtask";
            producedMessage.Message = message.ToString();
            string[] msg = message.ToString().Split(':');
            Guid id = Guid.Parse(msg[1]);
            producedMessage.Guid = id;
            producedMessage.ConsumerName = "subscriber1";

            DatabaseService db = DatabaseService.GetInstance();
            await db.LogDb(producedMessage);

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
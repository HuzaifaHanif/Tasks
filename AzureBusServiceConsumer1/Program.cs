using AzureBusServiceConsumer1.Models;
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts.AzureBusService;
using Services;
using Services.AzureBusService;

public class Program
{
    private readonly IAzureBusConsumerService _azureBusService;
    private static AzureBusServiceContext _dbcontext;

    public Program(IAzureBusConsumerService azureBusService , AzureBusServiceContext dbcontext)
    {
        _azureBusService = azureBusService;
        _dbcontext = dbcontext;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAzureBusConsumerService, AzureBusConsumerService>() 
            .AddSingleton<AzureBusServiceContext>()
            .AddSingleton<Program>()
            .AddSingleton<CustomDelegates.LoggingAzureBusServiceMessages>(provider =>
            {
                return new CustomDelegates.LoggingAzureBusServiceMessages(AddMessageToDB);
            })
            .BuildServiceProvider();


        var program = serviceProvider.GetRequiredService<Program>();
        Console.WriteLine("\tAzure Bus Service\t.............Consumer 1.........");
        await program.MessagesHandler();
    }

    public async Task MessagesHandler()
    {
        await _azureBusService.ConsumeMessages(
            "Endpoint=sb://huzaifaservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XawyVwykXqUjUI79C3RVFzzgc47imiqTa+ASbH+o4TA=",
            "onboardingtask",
            "subscriber1"
        );

    }

    public static async Task AddMessageToDB(ConsumerAzureBus consumedMessage)
    {
        await _dbcontext.AzureBusService.AddAsync(new AzureBus
        {
            Guid = consumedMessage.Guid,
            Topic = consumedMessage.Topic,
            Message = consumedMessage.Message,
            ConsumerName = consumedMessage.ConsumerName
        });

        await _dbcontext.SaveChangesAsync();
    }
}
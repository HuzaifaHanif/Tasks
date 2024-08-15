using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

public class Program
{
    private readonly IAzureBusService _azureBusService;

    public Program(IAzureBusService azureBusService)
    {
        _azureBusService = azureBusService;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAzureBusService, AzureBusService>() 
            .AddSingleton<Program>() 
            .AddSingleton<IAzureBusServiceDatabase , AzureBusServiceDatabase>()
            .AddSingleton<CustomDelegates.LoggingAzueBusServiceMessages>(provider =>
            {
                var dbService = provider.GetRequiredService<IAzureBusServiceDatabase>();
                return new CustomDelegates.LoggingAzueBusServiceMessages(dbService.LogConsumerData);
            })
            .BuildServiceProvider();

        //var databaseService = serviceProvider.GetRequiredService<IAzureBusServiceDatabase>();
        //var logDelegate = new CustomDelegates.LoggingMessagesToDB(databaseService.LogConsumerData);
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
}
﻿using AzureBusServiceConsumer2.Models;
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts.AzureBusService;
using Services;
using Services.AzureBusService;

public class Program
{
    private readonly IAzureBusConsumerService _azureBusService;
    private static AzureBusServiceContext _dbcontext;

    public Program(IAzureBusConsumerService azureBusService, AzureBusServiceContext dbcontext)
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

        //var databaseService = serviceProvider.GetRequiredService<IAzureBusServiceDatabase>();
        //var logDelegate = new CustomDelegates.LoggingMessagesToDB(databaseService.LogConsumerData);
        var program = serviceProvider.GetRequiredService<Program>();

        Console.WriteLine("\tAzure Bus Service\t.............Consumer 2.........");
        await program.MessagesHandler();
    }

    public async Task MessagesHandler()
    {
        await _azureBusService.ConsumeMessages(
            "connection string",
            "Topic name",
            "subscriber2"
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
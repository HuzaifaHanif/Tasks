 using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

public class Program
{
    private readonly IRabbitMQService _rabbitMQService;

    public Program(IRabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IRabbitMQService, RabbitMqService>()
            .AddSingleton<Program>()
            .AddSingleton<IRabbitMQServiceDatabase, RabbitMQServiceDatabase>()
            .AddSingleton<CustomDelegates.LoggingRabbitMQServiceMessages>(provider =>
            {
                var dbService = provider.GetRequiredService<IRabbitMQServiceDatabase>();
                return new CustomDelegates.LoggingRabbitMQServiceMessages(dbService.LogConsumerData);
            })
            .BuildServiceProvider();

        var program = serviceProvider.GetRequiredService<Program>();

        Console.WriteLine("\t\t\tRabbitMQConsumer 1\n\n Waiting For Meassges...\n\n");
        await program.MessagesHandler();
    }

    public async Task MessagesHandler()
    {
        string url = "amqp://user:password@localhost:5672";

        string queueName = "queue1";
        string exchange = "Huzaifa";
        string consumerName = "consumer1";

        await _rabbitMQService.ConsumeMessages(url , queueName , exchange , consumerName);
    }
}
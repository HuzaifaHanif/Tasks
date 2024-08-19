using Microsoft.Extensions.DependencyInjection;
using RabbitMQConsumer1;
using RabbitMQConsumer1.Models;
using ServiceContracts.RabbitMQService;
using Services;
using Services.RabbitMQService;

public class Program
{
    private readonly IRabbitMQConsumerService _rabbitMQService;
    //private static RabbitMQServiceContext _dbContext;

    public Program(IRabbitMQConsumerService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
        //_dbContext = dbContext;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IRabbitMQConsumerService, RabbitMQConsumerService>()
            .AddDbContext<RabbitMQServiceContext>(ServiceLifetime.Scoped)
            .AddSingleton<Program>()
            .AddScoped<Repository>()
            .AddTransient<CustomDelegates.LoggingRabbitMQServiceMessages>(provider =>
            {
                var repository = provider.GetService<Repository>();
                return new CustomDelegates.LoggingRabbitMQServiceMessages(repository.AddMessageToDB);
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

    //public static async Task AddMessageToDB(ConsumerRabbitMq consumedMessage)
    //{
    //    await _dbContext.RabbitMqs.AddAsync(new RabbitMq
    //    {
    //        Guid = consumedMessage.Guid,
    //        Queue = consumedMessage.Queue,
    //        Message = consumedMessage.Message,
    //        Exchange = consumedMessage.Exchange,
    //        ConsumerName = consumedMessage.ConsumerName,
    //    });

    //    await _dbContext.SaveChangesAsync();
    //}
}
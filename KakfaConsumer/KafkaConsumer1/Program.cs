
using KafkaConsumer1.Models;
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts.KafkaService;
using Services;
using Services.KafkaService;

public class Program
{
    private readonly IKafkaConsumerService _kafkaService;
    private static KafkaServiceContext _dbContext;

    public Program(IKafkaConsumerService kafkaService , KafkaServiceContext dbContext)
    {
        _kafkaService = kafkaService;
        _dbContext = dbContext;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IKafkaConsumerService, KafkaConsumerService>()
            .AddDbContext<KafkaServiceContext>()
            .AddSingleton<Program>()
            .AddSingleton<CustomDelegates.LoggingKafkaServiceMessages>(provider =>
            {
                return new CustomDelegates.LoggingKafkaServiceMessages(AddMessageToDB);
            })
            .BuildServiceProvider();


        var program = serviceProvider.GetRequiredService<Program>();

        Console.WriteLine("\tKafka\t.............Consumer 1.........");
        await program.MessagesHandler();
    }

    public async Task MessagesHandler()
    {
        KafkaConfig config = new KafkaConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "huzaifa1",
        };

        string topic = "KafkaPublisher";
        string consumerName = config.GroupId;

        await _kafkaService.ConsumeMessages(config, topic, consumerName);
    }

    public static async Task AddMessageToDB(ConsumerKafka consumedMessage)
    {
        await _dbContext.Kafkas.AddAsync(new Kafka
        {
            Guid = consumedMessage.Id,
            Topic = consumedMessage.Topic,
            Message = consumedMessage.Message,
            Partition = consumedMessage.Partition,
            ConsumerName = consumedMessage.ConsumerName, 
        });

        await _dbContext.SaveChangesAsync();
    }
}
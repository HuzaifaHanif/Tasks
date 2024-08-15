
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

public class Program
{
    private readonly IKafkaService _kafkaService;

    public Program(IKafkaService kafkaService)
    {
        _kafkaService = kafkaService;
    }

    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IKafkaService, KafkaService>()
            .AddSingleton<Program>()
            .AddSingleton<IKafkaServiceDatabase, KafkaServiceDatabase>()
            .AddSingleton<CustomDelegates.LoggingKafkaServiceMessages>(provider =>
            {
                var dbService = provider.GetRequiredService<IKafkaServiceDatabase>();
                return new CustomDelegates.LoggingKafkaServiceMessages(dbService.LogConsumerData);
            })
            .BuildServiceProvider();

        var program = serviceProvider.GetRequiredService<Program>();

        Console.WriteLine("\tKafka\t.............Consumer 2.........");
        await program.MessagesHandler();
    }

    public async Task MessagesHandler()
    {
        KafkaConfig config = new KafkaConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "huzaifa2",
        };

        string topic = "KafkaPublisher";
        string consumerName = config.GroupId;

        await _kafkaService.ConsumeMessages(config, topic, consumerName);
    }
}
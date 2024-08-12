using Task8.Models.Kafka;
using Task8.Models.RabbitMq;

namespace Task8.Repository.IRepository
{
    public interface IRabbitMQRepository
    {
        public Task AddMessagesAsync(List<RabbitMq> messages);

        public Task<List<RabbitMq>> GetAllMessagesAsync();
    }
}

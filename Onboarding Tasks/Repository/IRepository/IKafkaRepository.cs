using Task8.Models.Kafka;

namespace Task8.Repository.IRepository
{
    public interface IKafkaRepository
    {
        public Task AddMessagesAsync(List<Kafka> messages);

        public Task<List<Kafka>> GetAllMessagesAsync();
    }
}

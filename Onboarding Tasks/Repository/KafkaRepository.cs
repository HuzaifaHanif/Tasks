using Microsoft.EntityFrameworkCore;
using Task8.Data;
using Task8.Models.Kafka;
using Task8.Repository.IRepository;

namespace Task8.Repository
{
    public class KafkaRepository : IKafkaRepository
    {
        private readonly SoftechWorldWideContext _context;

        public KafkaRepository(SoftechWorldWideContext context)
        {
            _context = context;
        }

        public async Task AddMessagesAsync(List<Kafka> messages)
        {
            _context.Kafkas.AddRange(messages);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Kafka>> GetAllMessagesAsync()
        {
            return await _context.Kafkas.ToListAsync();
        }
    }
}

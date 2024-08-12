using Microsoft.EntityFrameworkCore;
using Task8.Data;
using Task8.Models.RabbitMq;
using Task8.Repository.IRepository;

namespace Task8.Repository
{
    public class RabbitMQRepository : IRabbitMQRepository
    {
        private readonly SoftechWorldWideContext _context;

        public RabbitMQRepository(SoftechWorldWideContext context)
        {
            _context = context;
        }

        public async Task AddMessagesAsync(List<RabbitMq> messages)
        {
            await _context.RabbitMQs.AddRangeAsync(messages);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<List<RabbitMq>> GetAllMessagesAsync()
        {
            return await _context.RabbitMQs.ToListAsync();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using RabbitMQConsumer1.Models;
using ServiceContracts.RabbitMQService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer1
{
    public class Repository
    {
        //private readonly RabbitMQServiceContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddMessageToDB(ConsumerRabbitMq consumedMessage)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RabbitMQServiceContext>();

                await dbContext.RabbitMqs.AddAsync(new RabbitMq
                {
                    Guid = consumedMessage.Guid,
                    Queue = consumedMessage.Queue,
                    Message = consumedMessage.Message,
                    Exchange = consumedMessage.Exchange,
                    ConsumerName = consumedMessage.ConsumerName,
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

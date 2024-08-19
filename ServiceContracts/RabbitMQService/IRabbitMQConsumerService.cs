using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.RabbitMQService
{
    public interface IRabbitMQConsumerService
    {
        Task ConsumeMessages(string Url, string queueName, string exchange, string consumerName);
    }
}

using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IRabbitMQService
    {
        List<ProducerRabbitMQ> ProduceMessages();

        Task ConsumeMessages(string Url, string queueName, string exchange , string consumerName);
    }
}

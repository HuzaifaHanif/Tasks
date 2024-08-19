using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.AzureBusService
{
    public interface IAzureBusProducerService
    {
        public Task<List<ProducerAzureBus>> ProduceMessages(string connectionString, string topicName);
    }
}

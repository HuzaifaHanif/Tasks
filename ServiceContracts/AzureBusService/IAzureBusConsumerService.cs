using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.AzureBusService
{
    public interface IAzureBusConsumerService
    {
        public Task ConsumeMessages(string connectionstring, string topic, string subscriberName);
    }
}

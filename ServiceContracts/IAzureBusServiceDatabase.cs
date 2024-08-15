using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IAzureBusServiceDatabase
    {
        Task LogConsumerData(ConsumerAzureBus consumerObj, string connectionString);
    }
}

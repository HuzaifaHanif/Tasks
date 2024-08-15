using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IKafkaServiceDatabase
    {
        Task LogConsumerData(ConsumerKafka consumerObj, string connectionString);
    }
}

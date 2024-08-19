using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.RabbitMQService
{
    public class ProducerRabbitMQ
    {
        public Guid Guid { get; set; }
        public string Queue { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

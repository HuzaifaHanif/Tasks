using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.KafkaService
{
    public class ProducerKafka
    {
        public Guid Guid { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

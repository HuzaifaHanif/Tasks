using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer1
{
    public class Kafka
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public string Message { get; set; }

        //public DateTime Timestamp { get; set; }

        public string ConsumerName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public class RabbitMq
    {
        public Guid Guid { get; set; }

        public string Queue { get; set; }

        public string Message { get; set; }

        public string Exchange { get; set; }

        //public DateTime Timestamp { get; set; }

        public string ConsumerName { get; set; }
    }
}

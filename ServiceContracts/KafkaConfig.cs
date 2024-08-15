using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }
    }
}

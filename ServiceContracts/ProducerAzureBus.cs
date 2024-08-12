using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public class ProducerAzureBus
    {
        public Guid Guid { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBusServiceConsumer2
{
    public class AzureBus
    {
        public Guid Guid { get; set; }

        public string Topic { get; set; }

        public string ConsumerName { get; set; }

        public string Message { get; set; }
    }
}

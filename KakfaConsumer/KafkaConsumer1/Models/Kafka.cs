﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafkaConsumer1.Models
{
    [Table("Kafka")]
    public partial class Kafka
    {
        
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public string Message { get; set; }
        public string ConsumerName { get; set; }
    }
}
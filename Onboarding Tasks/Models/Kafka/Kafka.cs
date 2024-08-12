using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task8.Models.Kafka
{
    [Table("Kafka")]
    public class Kafka
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public string Message { get; set; }

        //public DateTime Timestamp { get; set; }

        public string ConsumerName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task8.Models.RabbitMq
{
    [Table("RabbitMQ")]
    public class RabbitMq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Queue { get; set; }

        public string Message { get; set; }

        public string Exchange { get; set; }

        //public DateTime Timestamp { get; set; }

        public string ConsumerName { get; set; }
    }
}

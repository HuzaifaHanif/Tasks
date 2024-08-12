using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Models.AzureBusService
{
    [Table("AzureBus")]
    public class AzureBus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Topic { get; set; }

        public string ConsumerName { get; set; }

        public string Message { get; set; }
    }
}

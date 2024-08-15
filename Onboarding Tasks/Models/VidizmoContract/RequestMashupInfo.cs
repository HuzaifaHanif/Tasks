using static Nest.JoinField;

namespace Tasks.Models.VidizmoContract
{
    public class RequestMashupInfo
    {
        // Mashup Table

        public long? MashupId { get; set; }

        public long? UserId { get; set; }

        public long? TenantId { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? Content { get; set; }

        public bool? IsTranscoded { get; set; }

        public bool? IsAIProcessed { get; set; }   


        // MashupMultiLingualText Table


        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Culture { get; set; }

        public string? Tags  { get; set; }

        public string? Category { get; set; }

    }
}

namespace Tasks.Models.VidizmoContract
{
    public class MashupInfo
    {
        // Mashup Table

        public long MashupId { get; set; }

        public long UserId { get; set; }

        public long TenantId { get; set; }

        public long? version { get; set; }

        public long? Size { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? Content { get; set; }


        // MashupMultiLingualText Table


        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string Culture { get; set; }

    }
}

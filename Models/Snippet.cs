using System.ComponentModel.DataAnnotations;

namespace PasteIt.Models
{
    public class Snippet
    {

        [Key]
        public required string Id { get; set; }

        public string? Title { get; set; }

        public int ViewCount { get; set; }

        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ExpiresAt { get; set; }

    }
}

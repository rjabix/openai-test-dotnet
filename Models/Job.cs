using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using test_dotnet.Services;
using Pgvector.EntityFrameworkCore;
using Pgvector;

namespace test_dotnet.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Column("title")]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        //public string? Status { get; set; }

        [Column("description_embedding", TypeName = "vector(1536)")]
        [Required]
        public Vector? DescriptionEmbedding { get; set; }

        public static async Task<Job> CreateJobAsync(EmbeddingService service, string title, string description)
        {
            var descriptionEmbedding = await service.GetEmbeddingAsync(description);
            return new Job { Title = title, Description = description, DescriptionEmbedding = descriptionEmbedding };
        }
    }
}

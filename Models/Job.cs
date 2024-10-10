using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using test_dotnet.Services;

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
        public Vector<float>? DescriptionEmbedding { get; set; }

        public async Task<Job> CreateJobAsync(EmbeddingService service, string title, string description)
        {
            var descriptionEmbedding = await service.GetEmbeddingAsync(description);
            return new Job { Title = title, Description = description, DescriptionEmbedding = descriptionEmbedding };
        }
    }
}

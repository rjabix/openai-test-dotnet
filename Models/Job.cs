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
        [Column("portal")]
        public Portal Portal { get; set; }

        [Required]
        public int PortalId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ValidUntil { get; set; }

        [Column("description_embedding", TypeName = "vector(1536)")]
        [Required]
        public Vector? DescriptionEmbedding { get; set; }

        public static async Task<Job>? CreateJobAsync(EmbeddingService service, string title, string description, Portal portal, int portalId, DateTime validUntil)
        {
            if(DateTime.Now.AddDays(14) > validUntil)
            {
                Console.WriteLine($"[JOB_SKIPPED] [{portal.ToFriendlyString()} / {portalId}]");
                return null;
            }
            var descriptionEmbedding = await service.GetEmbeddingAsync(description);
            return new Job { Title = title, Description = description, DescriptionEmbedding = descriptionEmbedding,  Portal = portal, PortalId=portalId, ValidUntil=validUntil.ToString("MM/dd/yy") };
        }
    }
}

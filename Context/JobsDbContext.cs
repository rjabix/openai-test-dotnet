using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pgvector.EntityFrameworkCore;
using System.Reflection.Emit;
using test_dotnet.Models;
using test_dotnet.Services;

namespace test_dotnet.Context
{
    public class JobsDbContext(DbContextOptions<JobsDbContext> options, EmbeddingService embeddingService) : DbContext(options)
    {
        public DbSet<Job> Jobs { get; set; }


        private readonly EmbeddingService _embeddingService = embeddingService;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("vector");
        }

        public async Task<List<Job>> SearchAsync(string searchText, int page)
        {
            // this threshold is kinda like the accuracy of the search.
            // I would recommend to start with 0.5, which shoud
            // give you something. Normally 0.5 gives you too
            // much info. Tweaking it to the sweet spot is a fun experience.
            // it is different for any data.
            const double threshold = 0.50;

            // Generate embedding for the search text using OpenAI
            var queryEmbedding = await _embeddingService.GetEmbeddingAsync(searchText);

            // find neighbors in vector space and only take 5.
            // it also orders based on title embedding to show relevance of the order
            var offers = await Jobs
                .Where(job => job.DescriptionEmbedding!.L2Distance(queryEmbedding) < threshold)
                .OrderBy(post => post.DescriptionEmbedding!.L2Distance(queryEmbedding) < threshold)
                .Skip(page * 20)
                .Take(20)
                .ToListAsync();

            return offers;
        }
    }
}

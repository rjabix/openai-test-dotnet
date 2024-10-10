using OpenAI.Embeddings;
using System.Numerics;
using test_dotnet.Context;

namespace test_dotnet.Services
{
    public class EmbeddingService(JobsDbContext context, ILogger<EmbeddingService> logger, IConfiguration config)
    {
        private readonly ILogger<EmbeddingService> _logger = logger;
        private readonly JobsDbContext _context = context;
        private readonly EmbeddingClient _client = new("text-embedding-3-small", config.GetValue<string>("OPENAI_KEY"));

        public async Task<Vector<float>> GetEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding = await _client.GenerateEmbeddingAsync(text) ?? throw new Exception("Failed to generate embedding");
            ReadOnlyMemory<float> vector = embedding.ToFloats();
            return new Vector<float>(vector.Span);
        }
    }
}

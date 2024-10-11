using OpenAI.Embeddings;
using Pgvector;
using test_dotnet.Context;

namespace test_dotnet.Services
{
    public class EmbeddingService
    {
        private readonly ILogger<EmbeddingService> _logger;
        private readonly JobsDbContext _context;
        private readonly EmbeddingClient _client;

        public EmbeddingService(JobsDbContext context, ILogger<EmbeddingService> logger, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _client = new EmbeddingClient("text-embedding-3-small", config.GetValue<string>("OPENAI_KEY"));
        }

        public async Task<Vector> GetEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding = await _client.GenerateEmbeddingAsync(text) ?? throw new Exception("Failed to generate embedding");
            ReadOnlyMemory<float> vector = embedding.ToFloats();
            Console.WriteLine($"Embedding: {vector}");
            return new Vector(vector);
        }
    }
}

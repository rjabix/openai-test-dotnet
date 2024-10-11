using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Chat;
using Pgvector;
using test_dotnet.Context;
using test_dotnet.Models;
using test_dotnet.Services;

namespace test_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly ILogger<AIController> _logger;
    private readonly ChatClient client;
    private readonly JobsDbContext _context;
    private readonly EmbeddingService _embeddingService;
    public AIController(ILogger<AIController> logger, IConfiguration config, JobsDbContext context, EmbeddingService embeddingService)
    {
        _logger = logger;
        client = new OpenAIClient(config.GetValue<string>("OPENAI_KEY")).GetChatClient("gpt-4o-mini");
        _context = context;
        _embeddingService = embeddingService;
    }

    [HttpPost("GetAiResponse")]
    public async Task<IActionResult> Get([FromBody] ChatRequest request)
    {
        ChatCompletionOptions options = new();
        options.MaxOutputTokenCount = 2;
        ChatMessage message = new UserChatMessage(request.Prompt);
        ChatCompletion completion = client.CompleteChat([message], options);

        Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

        return Ok(completion.Content[0].Text);
    }

    public class ChatRequest
    {
        public required string Prompt { get; set; }
    }

    [HttpPost("CreateJob")]
    public async Task<IActionResult> CreateJob([FromBody] JobRequest jobrequest)
    {
        Job job = await Job.CreateJobAsync(_embeddingService, jobrequest.Title, jobrequest.Description);
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return Ok(job);
    }

    public class JobRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    [HttpPost("SearchJobs")]
    public async Task<IActionResult> SearchJobs([FromBody] ChatRequest searchRequest) // currently using ChatRequest as it also has 1 string field
    {
        Vector queryEmbedding = await _embeddingService.GetEmbeddingAsync(searchRequest.Prompt);
        List<Job> jobs = await _context.SearchAsync(queryEmbedding, 0);
        var results = jobs.Select(job => new { job.Title, job.Description });
        return Ok(results);
    }

}

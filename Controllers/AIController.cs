using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Chat;
using test_dotnet.Context;
using test_dotnet.Models;

namespace test_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly ILogger<AIController> _logger;
    private readonly ChatClient client;
    private readonly JobsDbContext _context;

    public AIController(ILogger<AIController> logger, IConfiguration config, JobsDbContext context)
    {
        _logger = logger;
        client = new OpenAIClient(config.GetValue<string>("OPENAI_KEY")).GetChatClient("gpt-4o-mini");
        _context = context;
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

}

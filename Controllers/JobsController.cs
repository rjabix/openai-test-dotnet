﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_dotnet.Context;
using test_dotnet.Models;
using test_dotnet.Services;

namespace test_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly JobsDbContext _context;
    private readonly EmbeddingService _embeddingService;

    public JobsController(JobsDbContext context, EmbeddingService embeddingService)
    {
        _context = context;
        _embeddingService = embeddingService;
    }

    [HttpPost("CreateJob")]
    public async Task<IActionResult> CreateJob([FromBody] JobRequest jobRequest)
    {
        try
        {
            var job = await Job.CreateJobAsync(_embeddingService, jobRequest.Title, jobRequest.Description, PortalExtensions.FromString(jobRequest.Portal), jobRequest.PortalId, DateTime.Parse(jobRequest.ValidUntil));
            if (job == null)
            {
                return Accepted("Too soon to expire, adding omitted.");
            }

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return Ok("Added successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class JobRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Portal { get; set; }
        public int PortalId { get; set; }
        public string ValidUntil { get; set; }
    }

    [HttpGet("GetJob")]
    public async Task<IActionResult> GetJob([FromQuery] string portal, [FromQuery] int id)
    {
        Job job = await _context.Jobs.FirstAsync(job => job.Portal == PortalExtensions.FromString(portal) && job.PortalId == id) ?? throw new Exception("Job cannot be found.");
        return Ok(job);
    }
}


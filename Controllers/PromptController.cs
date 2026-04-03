using Microsoft.AspNetCore.Mvc;
using QuantumFlow.MVC.Services;

namespace QuantumFlowMVC.Controllers;

public class PromptController : Controller
{
  private readonly ILogger<PromptController> _logger;
  private readonly IConfiguration _configuration;
  private readonly IGeminiService _geminiService;

  public PromptController(ILogger<PromptController> logger, IConfiguration configuration, IGeminiService geminiService)
  {
    _logger = logger;
    _configuration = configuration;
    _geminiService = geminiService;
  }

  public IActionResult Index()
  {
    return View();
  }

[HttpPost]
public async Task<IActionResult> AskGemini(string userPrompt)
{
    if (string.IsNullOrWhiteSpace(userPrompt))
    {
        return BadRequest("Prompt cannot be empty.");
    }

    try
    {
        // Call our service layer
        var aiResponse = await _geminiService.GenerateTextAsync(userPrompt);

        // Return clean JSON to the frontend
        return Ok(new { result = aiResponse });
    }
    catch (Exception ex)
    {
        // In a production environment, you'd log this error
        return StatusCode(500, "An error occurred while generating content.");
    }
}

}
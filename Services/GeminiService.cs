using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using QuantumFlow.MVC.Models;

namespace QuantumFlow.MVC.Services;

public interface IGeminiService
{
    Task<string> GenerateTextAsync(string prompt);
}

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    // Inject HttpClient and Configuration
    public GeminiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var apiKey = _config["Gemini:ApiKey"];
        var endpoint = $"{_config["Gemini:Endpoint"]}?key={apiKey}";

        var requestData = new GeminiRequest
        {
            Contents = new List<Content>
            {
                new Content
                {
                    Parts = new List<Part> { new Part { Text = prompt } }
                }
            },
            // Enable Google Search Grounding
            Tools = new List<Tool>
            {
                new Tool { GoogleSearch = new GoogleSearchTool() }
            }
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, jsonOptions), Encoding.UTF8, "application/json");

        // 2. Make the HTTP POST request to Google
        var response = await _httpClient.PostAsync(endpoint, jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini API error: {response.StatusCode} - {error}");
        }

        // 3. Parse the JSON response
        var responseJson = await response.Content.ReadAsStringAsync();
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson, jsonOptions);

        // Extract the actual text from the nested JSON structure
        return geminiResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No response generated.";
    }
}
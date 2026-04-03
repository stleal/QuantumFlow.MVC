using System.Text.Json.Serialization;
using System.Collections.Generic;

public class GeminiRequest
{
    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; } = new();

    // Add the Tools array to the request
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Tool> Tools { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

// The new Tool classes for Grounding
public class Tool
{
    [JsonPropertyName("googleSearch")]
    public GoogleSearchTool? GoogleSearch { get; set; }
}

public class GoogleSearchTool
{
    // Simply initializing this object as empty `{}` in the JSON
    // is enough to activate the feature.
}
using System.Text.Json.Serialization;

namespace TeleLlama.OllamaLogic;

public class OllamaRunningModelsResponse
{
    [JsonPropertyName("models")]
    public List<Model> Models { get; set; } = new List<Model>();
}

public class Model
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string ModelName { get; set; } = string.Empty;

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public ModelDetails Details { get; set; } = new ModelDetails();

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("size_vram")]
    public long SizeVram { get; set; }
}

public class ModelDetails
{
    [JsonPropertyName("parent_model")]
    public string ParentModel { get; set; } = string.Empty;

    [JsonPropertyName("format")]
    public string Format { get; set; } = string.Empty;

    [JsonPropertyName("family")]
    public string Family { get; set; } = string.Empty;

    [JsonPropertyName("families")]
    public List<string> Families { get; set; } = new List<string>();

    [JsonPropertyName("parameter_size")]
    public string ParameterSize { get; set; } = string.Empty;

    [JsonPropertyName("quantization_level")]
    public string QuantizationLevel { get; set; } = string.Empty;
}
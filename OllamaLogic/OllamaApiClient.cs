using System.Text;
using System.Text.Json;

namespace TeleLlama.OllamaLogic;

public class OllamaApiClient
{
    public static OllamaApiClient? Instance { get; private set; }
    public string RunningModel { get; private set; } = "";
    
    private HttpClient _httpClient = new();

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
    public void Initialize()
    {
        _httpClient.BaseAddress = new Uri("http://localhost:11434");
        Instance = this;
    }

    public async Task RunModel(string modelName)
    {
        if(await GetRunningModel() == modelName) return;
        
        var requestData = new
        {
            model = modelName,
            prompt = "This is a test prompt, please do not respond",
            system = "Do not respond",
            stream = false 
        };

        string jsonContent = JsonSerializer.Serialize(requestData);
        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("/api/generate", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string> GetRunningModel()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/ps");
        response.EnsureSuccessStatusCode();
        string responseString = await response.Content.ReadAsStringAsync();
        OllamaRunningModelsResponse? responseObject =
            JsonSerializer.Deserialize<OllamaRunningModelsResponse>(responseString, _options);
        
        if (responseObject == null || responseObject.Models.Count < 1)
        {
            return "";
        }

        return responseObject.Models[0].ModelName; // Ignoring if they are many
    } 
    
    public async Task<List<string>> GetModelList()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/tags");
        response.EnsureSuccessStatusCode();
        string responseString = await response.Content.ReadAsStringAsync();
        OllamaRunningModelsResponse? responseObject =
            JsonSerializer.Deserialize<OllamaRunningModelsResponse>(responseString, _options);
        
        List<string> list = new();
        
        if (responseObject == null || responseObject.Models.Count < 1)
        {
            return list;
        }

        foreach (Model model in responseObject.Models)     
        {
            list.Add(model.ModelName);
        }

        return list;
    } 
}
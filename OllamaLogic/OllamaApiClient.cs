using System.Text;
using System.Text.Json;
using Telegram.Bot.Types;
using TeleLlama.TelegramLogic;

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

    public async Task RunModel(string modelName, Chat? notificationReceiver)
    {
        if (await GetRunningModel() == modelName)
        {
            if (notificationReceiver != null)
            {
                TeleLlamaBot.Instance?.Send(notificationReceiver, "Model is already running");
            }
            
            return;
        }

        bool isPresent = false;
        
        foreach (string modelFromList in await GetModelList())
        {
            isPresent = isPresent || modelName == modelFromList;
        }

        if (!isPresent)
        {
            if (notificationReceiver != null)
            {
                TeleLlamaBot.Instance?.Send(notificationReceiver, "No such model");
            }
            
            return;
        }
        
        var payload = new
        {
            model = modelName,
            prompt = "This is a test prompt, please do not respond",
            system = "Do not respond",
            stream = false 
        };

        string jsonContent = JsonSerializer.Serialize(payload);
        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("/api/generate", content);
        response.EnsureSuccessStatusCode();
        
        if (notificationReceiver != null)
        {
            TeleLlamaBot.Instance?.Send(notificationReceiver, "Model loaded");
        }
    }

    public async Task<string> GetRunningModel()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/ps");
        response.EnsureSuccessStatusCode();
        string responseString = await response.Content.ReadAsStringAsync();
        OllamaModels? responseObject =
            JsonSerializer.Deserialize<OllamaModels>(responseString, _options);
        
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
        OllamaModels? responseObject =
            JsonSerializer.Deserialize<OllamaModels>(responseString, _options);
        
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

    public async Task<Stream> GetResponseStream(OllamaChatItem chatItem)
    {
        var requestData = new
        {
            model = await GetRunningModel(),
            messages = chatItem.MessageList,
            stream = true
        };

        string jsonPayload = JsonSerializer.Serialize(requestData);
        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/chat", content);
        return await response.Content.ReadAsStreamAsync();
    }
}
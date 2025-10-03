using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama;

public static class TeleLlamaService
{
    private static TeleLlamaBot _bot = new();
    private static OllamaApiClient _client = new();
    public static void Initialize()
    {
        _bot.Initialize();
        _client.Initialize();
    }
    
    
}
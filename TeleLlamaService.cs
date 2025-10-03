using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama;

public static class TeleLlamaService
{
    private static TeleLlamaBot _bot = new();
    private static OllamaApiClient _client = new();

    private static Dictionary<long, OllamaChatItem> _idChatItem = new();
    public static void Initialize()
    {
        _bot.Initialize();
        _client.Initialize();
    }
    
    
}
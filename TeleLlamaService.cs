using System.Diagnostics;
using System.Text.Json;
using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama;

public static class TeleLlamaService
{
    private static int _minDelay = 100;
    private static TeleLlamaBot _bot = new();
    private static OllamaApiClient _client = new();
    private static Dictionary<long, OllamaChatItem> _idChatItem = new();
    public static void Initialize()
    {
        _bot.Initialize();
        _client.Initialize();
    }

    public static async Task ChatStep(Chat chat, string message)
    {
        if (!_idChatItem.ContainsKey(chat.Id))
        {
            _idChatItem.Add(chat.Id, new OllamaChatItem());
        }
        
        OllamaChatItem chatItem = _idChatItem[chat.Id];
        
        chatItem.AddNewMessage(message, "user");

        Stream stream = await _client.GetResponseStream(chatItem);
        StreamReader reader = new(stream);

        string completeResponse = "";
        
        string? line;
        bool isFirst = true;
        
        while ((line = await reader.ReadLineAsync()) != null)
        {
            string jsonData = line;
            
            if (jsonData.Trim() == "[DONE]") break;
            if (string.IsNullOrWhiteSpace(jsonData)) continue;
            
            try
            {
                using JsonDocument document = JsonDocument.Parse(jsonData);
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("message", out JsonElement newLine) &&
                    newLine.TryGetProperty("content", out JsonElement contentElement))
                {
                    string? chunk = contentElement.GetString();

                    if (isFirst)
                    {
                        await _bot.Send(chat, chunk ?? "");
                        isFirst = false;
                    }
                    else
                    {
                        await _bot.AddToLastMessage(chat, chunk ?? "");
                    }

                    completeResponse += chunk;
                }
            }
            catch (JsonException e) { }
        }
        
        chatItem.AddNewMessage(completeResponse, "assistant");
    }

    public static void SetNewSystemPrompt(long id, string prompt)
    {
        _idChatItem[id].SystemPrompt = prompt;
    }

    public static void ClearChat(long id)
    {
        _idChatItem.Remove(id);
    }

    public static bool IsInConversation(long id)
    {
        return _idChatItem.ContainsKey(id);
    }

    public static void StartConversation(long id)
    {
        _idChatItem.Add(id, new OllamaChatItem());
    }
}
using TeleLlama.OllamaLogic;

namespace TeleLlama;

public class OllamaChatItem
{
    public List<OllamaMessage> MessageList { get; } = new();
    private string _systemPrompt = "";

    public string SystemPrompt
    {
        set => _systemPrompt = value ?? "";
    }

    public void AddNewMessage(string message, string role)
    {
        if (role == "user")
        {
            AddNewMessage(_systemPrompt, "system");
        }
        
        OllamaMessage newMessage = new();
        newMessage.Content = message;
        newMessage.Role = role;
        MessageList.Add(newMessage);
    }
}
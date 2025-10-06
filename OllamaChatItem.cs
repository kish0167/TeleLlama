using TeleLlama.OllamaLogic;

namespace TeleLlama;

public class OllamaChatItem
{
    public List<OllamaMessage> MessageList { get; } = new();

    public void AddNewMessage(string message, string role)
    {
        OllamaMessage newMessage = new();
        newMessage.Content = message;
        newMessage.Role = role;
        MessageList.Add(newMessage);
    }
}
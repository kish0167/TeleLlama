using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama.Commands;

public class GetModelsListCommand : Command
{
    public GetModelsListCommand()
    {
        Syntax = "/getModelsList";
        Description = "Get list of available models";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        Task.Run(() => GetAndSendList(msg));
    }

    private async Task GetAndSendList(Message msg)
    {
        List<string> list = await OllamaApiClient.Instance?.GetModelList()!;
        string text = "List of available models:\n\n";
        
        foreach (string s in list)
        {
            text += s + "\n";
        }

        TeleLlamaBot.Instance?.Send(msg.Chat, text);
    }
}
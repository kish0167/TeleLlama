using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama.Commands;

public class GetRunningModelCommand : Command
{
    public GetRunningModelCommand()
    {
        Syntax = "/getRunningModel";
        Description = "Get model name that is running now";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        Task.Run(() => GetAndSendModel(msg));
    }
    
    private async Task GetAndSendModel(Message msg)
    {
        string runningModel = await OllamaApiClient.Instance?.GetRunningModel()!;
        string text;
        
        if (runningModel == "")
        {
            text = "No model is currently running";
        }
        else
        {
            text = runningModel + "  <-  is running now";
        }
        
        TeleLlamaBot.Instance?.Send(msg.Chat, text);
    }
}
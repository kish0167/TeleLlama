using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;
using TeleLlama.TelegramLogic;

namespace TeleLlama.Commands;

public class RunModelCommand : Command
{
    public RunModelCommand()
    {
        Syntax = "/run";
        Description = "Run specific model";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        if (args.Length < 1)
        {
            TeleLlamaBot.Instance?.Send(msg.Chat, "/run 'model'");
            return;
        }
        
        Task.Run(() => TeleLlamaBot.Instance?.Send(msg.Chat,"Please, standby"));
        
        Task.Run(() => OllamaApiClient.Instance?.RunModel(args[0], msg.Chat));
    }
}
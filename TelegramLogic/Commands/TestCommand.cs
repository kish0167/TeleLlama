using Telegram.Bot.Types;
using TeleLlama.OllamaLogic;

namespace TeleLlama.TelegramLogic.Commands;

public class TestCommand : Command
{
    public TestCommand()
    {
        Syntax = "/test";
        Description = "this is for testing purposes";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        string text = "";
        foreach (string word in args)
        {
            text += word + " ";
        }
        
        Task.Run(() => TeleLlamaService.ChatStep(msg.Chat, text));
    }
}
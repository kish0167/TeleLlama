using Telegram.Bot.Types;
using TeleLlama.TelegramLogic;

namespace TeleLlama.Commands;

public class TestCommand : Command
{
    public TestCommand()
    {
        Syntax = "/test";
        Description = "this is for testing purposes";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        Task.Run(() => TeleLlamaBot.Instance?.Send(msg.Chat, "I said it's for testing only, you idiot"));
    }
}
using Telegram.Bot.Types;

namespace TeleLlama.Commands;

public class ClearChatCommand : Command
{
    public ClearChatCommand()
    {
        Syntax = "/clear";
        Description = "Clear conversation";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        TeleLlamaService.ClearChat(msg.Chat.Id);
    }
}
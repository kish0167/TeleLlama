using Telegram.Bot.Types;

namespace TeleLlama.TelegramLogic.Commands;

public class StartConversationCommand : Command
{
    public StartConversationCommand()
    {
        Syntax = "/start";
        Description = "Start conversation";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        TeleLlamaService.StartConversation(msg.Chat.Id);
    }
}
using Telegram.Bot.Types;
using TeleLlama.TelegramLogic;

namespace TeleLlama.Commands;

public class HelpCommand : Command
{
    public HelpCommand()
    {
        Syntax = "/help";
        Description = "Show available commands";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        string text = "";
        foreach (Command? command in CommandService.CommandSet)
        {
            if(command == null) continue;
            
            text += command.Syntax;
            text += " ";
            text += command.Description;
            text += "\n";
        }

        TeleLlamaBot.Instance?.Send(msg.Chat, text);
    }
}
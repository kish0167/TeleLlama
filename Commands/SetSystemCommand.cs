using Telegram.Bot.Types;

namespace TeleLlama.Commands;

public class SetSystemCommand : Command
{
    public SetSystemCommand()
    {
        Syntax = "/system";
        Description = "Set system prompt";
    }
    public override void PerformActions(string[] args, Message msg)
    {
        string prompt = "";
        
        foreach (string word in args)
        {
            prompt += word + " ";
        }
        
        TeleLlamaService.SetNewSystemPrompt(msg.Chat.Id, prompt);
    }
}
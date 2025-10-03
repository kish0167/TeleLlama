using Telegram.Bot.Types;

namespace TeleLlama.TelegramLogic.Commands;

public abstract class Command
{
    public string Syntax { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;

    public abstract void PerformActions(string[] args, Message msg);

    public static T Create<T>() where T : new()
    {
        return new T();
    }
}
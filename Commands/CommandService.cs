using System.Reflection;
using Telegram.Bot.Types;

namespace TeleLlama.Commands;

public static class CommandService
{
    public static readonly List<Command?> CommandSet = new();

    public static void Initialise()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(Command).IsAssignableFrom(type) && type != typeof(Command))
                {
                    Command command = Activator.CreateInstance(type) as Command;
                    CommandSet.Add(command);
                }
            }
        }
        
        CommandSet.Sort(SyntaxComparison);
    }

    private static int SyntaxComparison(Command x, Command y)
    {
        return String.CompareOrdinal(x.Syntax, y.Syntax);
    }
    
    public static bool TryToProcessCommand(Message msg)
    {
        string currentCommand = msg.Text ?? msg.Caption ?? " ";

        if (!currentCommand.StartsWith("/"))
        {
            return false;
        }
        
        string[] split = currentCommand.Split(' ');
        string opcode = split[0];

        string[] args = split.Length > 1 ? split[1..] : Array.Empty<string>();
        
        foreach (Command? command in CommandSet)
        {
            if (command == null || opcode != command.Syntax) continue;
            
            try
            {
                command.PerformActions(args, msg);
            }
            catch (Exception e)
            {
                return false;
            }
                
            return true;
        }
        
        new HelpCommand().PerformActions(args, msg);
        return true;
    }
}
namespace TeleLlama.TelegramLogic;
using Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Utils;

public class TeleLlamaBot
{
    public static TeleLlamaBot? Instance;
    
    private readonly CancellationTokenSource _cts = new();
    private TelegramBotClient? _bot;

    private List<long> _whitelist = new();
    private string _password = "1-icq23rq03094r-24vi34t3/wrm";
    private readonly Dictionary<long, (int MessageId, string Text)> _lastMessages = new();
    public async void Initialize()
    {
        string token = TxtHandler.ReadTxt("token.txt");

        try
        {
            _bot = new TelegramBotClient(token, cancellationToken: _cts.Token);
        }
        catch (Exception e)
        {
            _bot = null;
        }

        if (_bot != null)
        {
            Instance = this;
            CommandService.Initialise();
            await _bot.DropPendingUpdates();
            _bot.OnError += ErrorCallback;
            _bot.OnMessage += MessageCallback;
            _bot.OnUpdate += UpdateCallback;
        }

        ReadPassword();
        ReadWhitelist();
    }

    private async Task MessageCallback(Message msg, UpdateType type)
    {
        if (msg.Text == _password && !_whitelist.Contains(msg.Chat.Id))
        {
            _whitelist.Add(msg.Chat.Id);
            await Send(msg.Chat, "Approved!");
            SaveWhiteList();
        }

        if (!_whitelist.Contains(msg.Chat.Id))
        {
            await Send(msg.Chat, "Look at you, you were once so proud, go now and never return");
            return;
        }

        if(CommandService.TryToProcessCommand(msg)) return;

        if (TeleLlamaService.IsInConversation(msg.Chat.Id))
        {
            await TeleLlamaService.ChatStep(msg.Chat, msg.Text ?? "I just sent you very funny picture, " +
                "please lie to react like you saw it");
        }
    }

    private async Task UpdateCallback(Update update)
    {
        await Task.Delay(1);
    }
    
    private async Task ErrorCallback(Exception exception, HandleErrorSource source)
    {
        await Task.Delay(1);
    }
    
    public async Task Send(Chat chat, string text)
    {
        if (_bot == null)
        {
            return;
        }
        
        Message msg = await _bot.SendMessage(chat, text);
        UpdateLastMessageBuffer(chat, msg);
    }

    public async Task AddToLastMessage(Chat chat, string newTextPiece)
    {
        string newMessage = _lastMessages[chat.Id].Text + newTextPiece;
        await EditMessage(chat, _lastMessages[chat.Id].MessageId, newMessage);
        _lastMessages[chat.Id] = (_lastMessages[chat.Id].MessageId, newMessage);
    }
    
    private async Task EditMessage(Chat chat, int messageId, string newText)
    {
        if (_bot == null)
        {
            return;
        }

        await _bot.EditMessageText(new ChatId(chat.Id), messageId, newText);
    }
    
    private void UpdateLastMessageBuffer(Chat chat, Message msg)
    {
        _lastMessages[chat.Id] = (msg.MessageId, msg.Text ?? "");
    }
    
    private void SaveWhiteList()
    {
        TxtHandler.SaveAsObject(_whitelist, "whitelist");
    }

    private void ReadWhitelist()
    {
        _whitelist = TxtHandler.ReadAsObject<List<long>>("whitelist.txt") ?? new List<long>();
    }

    private void ReadPassword()
    {
        _password = TxtHandler.ReadTxt("password.txt");
        
        if (_password.Length <= 5)
        {
            _password = "1-icq23rq03094r-24vi34t3/wrm";
        }
    }
}
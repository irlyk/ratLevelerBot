using RatLevelerBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RatLevelerBot.Services;

public class BotCommandExecuter : IBotCommandExecuter
{
    private readonly ITelegramBotClient _botClient;

    private readonly IRatLevelerService _ratLevelrService; 

    private readonly ILogger<BotCommandExecuter> _logger;

    public BotCommandExecuter(
        ITelegramBotClient botClient,
        IRatLevelerService ratLevelrService,
        ILogger<BotCommandExecuter> logger
    )
    {
        _botClient = botClient;
        _ratLevelrService = ratLevelrService;
        _logger = logger;
    }

    public async Task StartCommand(Message message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Добро пожаловать!",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
    }

    public async Task NewRatCommand(Message message, CancellationToken cancellationToken)
    {
        _ratLevelrService.AddNewChatUser(message.From.Conver(), message.Chat.Convert());

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"В чате новая крыска - {message.From.FirstName}",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
        
    }

    public async Task GetLevelCommand(Message message, CancellationToken cancellationToken)
    {
        var userLevel = _ratLevelrService.GetUserLevelInChat(message.From.Id, message.Chat.Id);
        
        if (userLevel == null) 
        {
            _logger.LogError("/level userLevel is null");
            return;
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Ваш крысиный уровень {userLevel.Level.Value} - {userLevel.Level.Name} (опыт: {userLevel.Exp})",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
    }
    
    public async Task SetLevelCommand(Message message, CancellationToken cancellationToken) 
    {
        var splittedText = message.Text.Split();
        if (splittedText.Length != 2 || !Int32.TryParse(splittedText[1], out var levelValue)) {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Команда '/SetLevel' введена неверно!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
            return;
        }

        var level = _ratLevelrService.SetUserLevelInChat(message.From.Id, message.Chat.Id, levelValue);

        if (level == null) 
        {
            _logger.LogError("/setLevel userLevel is null");
            return;
        }


        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Ваш новый крысиный уровень {level.Value} - {level.Name})",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
    }

    public async Task ResetLevelCommand(Message message, CancellationToken cancellationToken)
    {
        var level = _ratLevelrService.ResetLevel(message.From.Id, message.Chat.Id);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Уровень крыски {message.From.GetPrettyName()} сброшен до {level.Name} ({level.Value})",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
        
    }

    public async Task IncreaseExpCommand(Message message, CancellationToken cancellationToken) 
    {
        // message not replied
        if (message.ReplyToMessage?.From == null) 
        {
            _logger.LogError("No replied user");
            await SendUnknownCommand(message, cancellationToken);
            return;
        }

        var replyUserId = message.ReplyToMessage.From.Id;
        var level = _ratLevelrService.IncreaseUserChatExp(replyUserId, message.Chat.Id, Consts.DefaultExpIncrease);

        if (level != null) 
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Уровень крыски {message.ReplyToMessage.From.GetPrettyName()} повышен до {level.Name}",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        }

    }
    
    public async Task SendUnknownCommand(Message message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Я не знаю такой команды(",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
    }

    public async Task SendErrorCommand(Message message, CancellationToken cancellationToken)
    {
        try {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Извините, произошла ошибка",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}


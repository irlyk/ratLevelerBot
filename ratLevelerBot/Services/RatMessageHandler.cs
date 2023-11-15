using RatLevelerBot.Helpers;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace RatLevelerBot.Services;

public class RatMessageHandler
{
    private readonly ILogger<RatMessageHandler> _logger;
    private readonly IBotCommandExecuter _botCommandExecuter;

    public RatMessageHandler(ILogger<RatMessageHandler> logger, IBotCommandExecuter botCommandExecuter)
	{
        _logger = logger;
        _botCommandExecuter = botCommandExecuter;
	}

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation($"HandleError: {ErrorMessage}");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.Message.IsInvalid())
        {
            _logger.LogError($"Message is invalid");
            return;
        }

        var handler = update switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            //{ EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            //{ CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            //{ InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            //{ ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _ => UnknownUpdateHandlerAsync(),
        };

        if (handler != null)
            await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var comandString = message.Text?.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

        // todo maybe log if not a command
        if (string.IsNullOrEmpty(comandString) || !BotCommandsMapper.BotCommandsList.Contains(comandString)) 
            return;

        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        _logger.LogInformation($"Chat {message.Chat.Id} comandString");

        var command = BotCommandsMapper.Map(comandString);

        try 
        {
            switch (command)
            {
                case BotCommands.Start:
                    await _botCommandExecuter.StartCommand(message, cancellationToken);
                    return;
                case BotCommands.ChatLevels:
                    await _botCommandExecuter.GetChatLevels(message, cancellationToken);
                    return;
                case BotCommands.Level:
                    await _botCommandExecuter.GetLevelCommand(message, cancellationToken);
                    return;
                case BotCommands.SetLevel:
                    await _botCommandExecuter.SetLevelCommand(message, cancellationToken);
                    return;
                case BotCommands.NewRat:
                    await _botCommandExecuter.NewRatCommand(message, cancellationToken);
                    return;
                case BotCommands.ResetLevel:
                    await _botCommandExecuter.ResetLevelCommand(message, cancellationToken);
                    return;
                case BotCommands.IncreaseExp:
                    await _botCommandExecuter.IncreaseExpCommand(message, cancellationToken);
                    return;
                case BotCommands.Unknown:
                    await _botCommandExecuter.SendUnknownCommand(message, cancellationToken);
                    return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await _botCommandExecuter.SendErrorCommand(message, cancellationToken);
        }
    }

    // todod rework Unknown update type handler
    private Task UnknownUpdateHandlerAsync() 
    {
        _logger.LogInformation("Unknown update type");
        return Task.CompletedTask;
    }
}




using Telegram.Bot.Types;

namespace RatLevelerBot.Services;

public interface IBotCommandExecuter
{
    Task StartCommand(Message message, CancellationToken cancellationToken);

    Task GetLevelCommand(Message message, CancellationToken cancellationToken);

    Task SetLevelCommand(Message message, CancellationToken cancellationToken);

    Task NewRatCommand(Message message, CancellationToken cancellationToken);

    Task ResetLevelCommand(Message message, CancellationToken cancellationToken);

    Task SendUnknownCommand(Message message, CancellationToken cancellationToken);

    Task SendErrorCommand(Message message, CancellationToken cancellationToken);
}

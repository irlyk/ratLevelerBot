using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using UserInternal = RatLevelerBot.Models.User;
using ChatInternal = RatLevelerBot.Models.Chat;
using UserLevelInternal = RatLevelerBot.Models.UserLevel;
using LevelInternal = RatLevelerBot.Models.Level;

namespace RatLevelerBot;

public static class WebHookExtensions
{
    public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
        where T : class
    {
        var o = serviceProvider.GetService<IOptions<T>>();
        if (o is null)
            throw new ArgumentNullException(nameof(T));

        return o.Value;
    }

    public static ControllerActionEndpointConventionBuilder MapBotWebhookRoute<T>(
        this IEndpointRouteBuilder endpoints,
        string route)
    {
        var controllerName = typeof(T).Name.Replace("Controller", "", StringComparison.Ordinal);
        var actionName = typeof(T).GetMethods()[0].Name;

        return endpoints.MapControllerRoute(
            name: "bot_webhook",
            pattern: route,
            defaults: new { controller = controllerName, action = actionName });
    }
}

public static class MessageExtensions
{
    public static bool IsInvalid(this Message? message)
    {
        if (message == null || message.Chat == null || message.From == null)
            return true;
        return false;
    }

    public static string GetPrettyName(this User user) 
    {
        var names = new List<string>(3);

        if (!string.IsNullOrWhiteSpace(user.FirstName))
            names.Add(user.FirstName);
        if (!string.IsNullOrWhiteSpace(user.LastName))
            names.Add(user.LastName);
        if (!string.IsNullOrWhiteSpace(user.Username))
            names.Add("(@" + user.Username + ")");

        return string.Join(" ", names);
    }

    public static UserInternal Conver(this User telegramUser) 
    {
        return new UserInternal {
            Id = telegramUser.Id,
            FirstName = telegramUser.FirstName,
            LastName = telegramUser.LastName
        };
    }

    public static ChatInternal Convert(this Chat telegramChat) 
    {
        string chatName;
        if (!string.IsNullOrEmpty(telegramChat.FirstName) && !string.IsNullOrEmpty(telegramChat.LastName))
            chatName = $"{telegramChat.FirstName} {telegramChat.LastName}";
        else 
            chatName = telegramChat.Title;

        return new ChatInternal {
            Id = telegramChat.Id,
            Name = chatName
        };
    }
}

public static class LevelExtensions 
{
    public static UserLevelInternal SetLevel(this UserLevelInternal userLevel, LevelInternal level) 
    {
        userLevel.Level = level;
        userLevel.Exp = level.Exp;
        return userLevel;
    }
}
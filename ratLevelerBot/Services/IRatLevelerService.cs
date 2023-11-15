using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public interface IRatLevelerService 
{
    List<UserLevel>? GetChatLevels(long chatId);

    UserLevel? GetUserLevelInChat(long userId, long chatId);

    Level? SetUserLevelInChat(long userId, long chatId, int levelValue);

    void AddNewChatUser(User user, Chat chat);

    Level? ResetLevel(long userId, long chatId);

    Level? IncreaseUserChatExp(long userId, long chatId, long exp);
}
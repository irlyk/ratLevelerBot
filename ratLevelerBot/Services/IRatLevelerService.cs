using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public interface IRatLevelerService 
{
    UserLevel? GetUserLevelInChat(long userId, long chatId);

    Level? SetUserLevelInChat(long userId, long chatId, int levelValue);

    void AddNewChatUser(User user, Chat chat);

    Level? ResetLevel(long userId, long chatId);
}
using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public class RatLevelerService : IRatLevelerService
{
    private readonly IUserRepository _userRepository;

    private readonly IChatRepository _chatRepository;

    private readonly ILevelRepository _levelRepository;

    public RatLevelerService(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        ILevelRepository levelRepository
    )
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _levelRepository = levelRepository;
    }

    public UserLevel? GetUserLevelInChat(long userId, long chatId) 
    {
        var chat = _chatRepository.GetById(chatId);
        if (chat == null)
            return null; // todo no such chat

        var userLevel = chat.UserLevels.FirstOrDefault(x => x.User.Id == userId);
        if (userLevel == null)
            return null; // todo no user in chat
        
        return userLevel;
    }

    public Level? SetUserLevelInChat(long userId, long chatId, int levelValue) 
    {
        var chat = _chatRepository.GetById(chatId);
        if (chat == null) // todo no chat found
            return null;
        
        var userLevel = chat.UserLevels.FirstOrDefault(x => x.User.Id == userId);
        if (userLevel == null) // todo no such user in chat 
            return null; 

        var level = _levelRepository.GetAll().FirstOrDefault(x => x.Value == levelValue);
        if (level == null) // todo no such level 
            return null; 
        
        userLevel.Level = level;
        userLevel.Exp = level.Exp;

        _chatRepository.Update(chat);
        _chatRepository.Save();

        return level;
    }

    public void AddNewChatUser(User user, Chat chat) 
    {
        var firstLevel = _levelRepository.GetAll().FirstOrDefault(x => x.Value == Level.FirstLevel);
        if (firstLevel == null) // todo no first level
            return;

        // if user doesn't exist -> add new user
        var dbUser = _userRepository.GetById(user.Id);
        if (dbUser == null) 
        {
            _userRepository.Insert(user);
            _userRepository.Save();
        }

        var dbChat = _chatRepository.GetById(chat.Id);

        // if chat not exist -> create new chat
        if (dbChat == null) 
        {
            chat.UserLevels.Add(new UserLevel {
                User = user,
                Level = firstLevel
            });
            _chatRepository.Insert(chat);
            _chatRepository.Save();
            return;
        }

        var userInChat = dbChat.UserLevels.FirstOrDefault(x => x.User.Id == user.Id);
        if (userInChat != null) // todo user in chat exist
            return;
        
        dbChat.UserLevels.Add(new UserLevel {
            User = user,
            Level = firstLevel
        });
        _chatRepository.Update(dbChat);
        _chatRepository.Save();
    }

    // todo implement reset level
    public Level ResetLevel(User user, Chat chat)
    {
        throw new NotImplementedException();
    }
}
using RatLevelerBot.Models;
using RatLevelerBot.Services.Repositories;

namespace RatLevelerBot.Services;

public class RatLevelerService : IRatLevelerService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Chat> _chatRepository;
    private readonly IRepository<Level> _levelRepository;
    private readonly IRepository<UserLevel> _userLevelRepository;

    public RatLevelerService(
        IRepository<User> userRepository,
        IRepository<Chat> chatRepository,
        IRepository<Level> levelRepository,
        IRepository<UserLevel> userLevelRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _levelRepository = levelRepository;
        _userLevelRepository = userLevelRepository;
    }

    public List<UserLevel>? GetChatLevels(long chatId) 
    {
        var chat = _chatRepository.GetById(chatId);
        
        if (chat == null) // todo chat doesn't exist
            return null;
        
        return chat.UserLevels.ToList();
    }

    public UserLevel? GetUserLevelInChat(long userId, long chatId) 
    {
        var user = _chatRepository.GetById(chatId);
        if (user == null) // todo user not exist
            return null;

        var userLevel = user.UserLevels.FirstOrDefault(ul => ul.User.Id == userId);
        if (userLevel == null)
            return null; // todo no user in chat
        
        return userLevel;
    }

    public Level? SetUserLevelInChat(long userId, long chatId, int levelValue) 
    {
        var level = _levelRepository.GetAll().FirstOrDefault(x => x.Value == levelValue);
        if (level == null) // todo no such level 
            return null; 
        
        var userLevel = _userLevelRepository.FindBy(ul => ul.User.Id == userId && ul.Chat.Id == chatId);
        if (userLevel == null) 
            return null; // todo UserLevel doesn't exists

        userLevel.SetLevel(level);

        _userLevelRepository.Update(userLevel);
        _userLevelRepository.Save();

        return level;
    }

    public Level? ResetLevel(long userId, long chatId)
    {
        var level = SetUserLevelInChat(userId, chatId, Level.FirstLevel);
        return level;
    }

    public void AddNewChatUser(User user, Chat chat) 
    {
        var firstLevel = _levelRepository.FindBy(x => x.Value == Level.FirstLevel);

        if (firstLevel == null) 
            return; // todo firstLevel doesn't exists

        var chatDb = _chatRepository.GetById(chat.Id);

        // if chat exist - update Chat
        if (chatDb == null) 
        {
            chat.UserLevels.Add(new UserLevel 
            {
                User = user,
                Level = firstLevel,
                Exp = firstLevel.Exp,
            });
            _chatRepository.Insert(chat);
        }
        else 
        {
            if (chatDb.UserLevels.FirstOrDefault(ul => ul.User.Id == user.Id) != null) 
                return; // todo UserLevel exists
            
            chat.UserLevels.Add(new UserLevel 
            {
                User = user,
                Level = firstLevel,
                Exp = firstLevel.Exp,
            });
            _chatRepository.Update(chat);
        }
        _chatRepository.Save();
    }

}
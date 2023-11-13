using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public interface IUserRepository : IDisposable
{
    IQueryable<User> GetAll();

    User? GetById(long levelId);

    void Insert(User user);

    void Update(User user);

    void Delete(long userId);

    void Save();
}


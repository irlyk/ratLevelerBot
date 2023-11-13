using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public interface ILevelRepository : IDisposable
{
    IQueryable<Level> GetAll();

    Level? GetById(int levelId);

    void Insert(Level level);

    void Update(Level level);

    void Delete(int levelId);

    void Save();
}


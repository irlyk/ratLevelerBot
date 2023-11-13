using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public interface IChatRepository : IDisposable
{
    IQueryable<Chat> GetAll();

    Chat? GetById(long chatId);

    void Insert(Chat chat);

    void Update(Chat chat);

    void Delete(long chatId);

    void Save();
}


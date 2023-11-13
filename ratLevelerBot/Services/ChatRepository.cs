using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public class ChatRepository : IChatRepository, IDisposable
{
    private ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context)
	{
        _context = context;
	}

    public IQueryable<Chat> GetAll() 
    {
        return _context.Chats;   
    }

    public Chat? GetById(long chatId) 
    {
        return _context.Chats.Find(chatId); 
    }

    public void Insert(Chat chat)
    {
        _context.Chats.Add(chat);
    }

    public void Update(Chat chat)
    {
        _context.Entry(chat).State = EntityState.Modified;
    }

    public void Delete(long chatId)
    {
        var chat = _context.Chats.Find(chatId);

        if(chat != null)
        {
            _context.Chats.Remove(chat);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    #region Dispose
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}


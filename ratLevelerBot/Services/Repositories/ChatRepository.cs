using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services.Repositories;

public class ChatRepository : IRepository<Chat>, IDisposable
{
    private readonly ApplicationDbContext _context;

    private readonly IQueryable<Chat> _chats;

    public ChatRepository(ApplicationDbContext context)
	{
        _context = context;
        _chats = _context.Chats
            .Include(c => c.UserLevels)
                .ThenInclude(ul => ul.User)
            .Include(c => c.UserLevels)
                .ThenInclude(ul => ul.Level);
	}

    public IQueryable<Chat> GetAll() => _chats;

    public Chat? GetById(long id) 
    {
        return _chats?.SingleOrDefault(c => c.Id == id);
    }

    public Chat? FindBy(Expression<Func<Chat, bool>> expression) {
        return _chats?.FirstOrDefault(expression);
    }

    public IQueryable<Chat>? FilterBy(Expression<Func<Chat, bool>> expression) {
        return _chats?.Where(expression);
    }

    public void Insert(Chat item)
    {
        _context.Chats.Add(item);
    }

    public void Update(Chat item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var chat = _context.Chats.Find(id);

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


using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public class UserRepository : IUserRepository, IDisposable
{
    private ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
	{
        _context = context;
	}

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }

    public User? GetById(long userId)
    {
        return _context.Users.Find(userId);
    }

    public void Insert(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public void Delete(long userId)
    {
        var user = _context.Users.Find(userId);

        if(user != null)
        {
            _context.Users.Remove(user);
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
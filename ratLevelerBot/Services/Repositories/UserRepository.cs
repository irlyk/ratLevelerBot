using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services.Repositories;

public class UserRepository : IRepository<User>, IDisposable
{
    private readonly ApplicationDbContext _context;

    private readonly IQueryable<User> _users;

    public UserRepository(ApplicationDbContext context)
	{
        _context = context;
        _users = _context.Users
                    .Include(u => u.UserLevels);
	}

    public IQueryable<User> GetAll() => _users;

    public User? GetById(long id)
    {
        return _users?.SingleOrDefault(u => u.Id == id);
    }

    public User? FindBy(Expression<Func<User, bool>> expression)
    {
        return _users?.FirstOrDefault(expression);
    }

    public IQueryable<User>? FilterBy(Expression<Func<User, bool>> expression)
    {
        return _users?.Where(expression);
    }

    public void Insert(User item)
    {
        _context.Users.Add(item);
    }

    public void Update(User item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var user = _context.Users.Find(id);

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
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services.Repositories;

public class UserLevelRepository : IRepository<UserLevel>, IDisposable
{
    private readonly ApplicationDbContext _context;

    private readonly IQueryable<UserLevel> _userLevels;
    
    public UserLevelRepository(ApplicationDbContext context)
	{
        _context = context;
        _userLevels = _context.UserLevels
                .Include(ul => ul.Chat)
                .Include(ul => ul.User)
                .Include(ul => ul.Level);
	}

    public IQueryable<UserLevel> GetAll() => _userLevels;

    public UserLevel? GetById(long id) 
    {
        return _userLevels?.SingleOrDefault(ul => ul.Id == id); 
    }

    public UserLevel? FindBy(Expression<Func<UserLevel, bool>> expression)
    {
        return _userLevels?.FirstOrDefault(expression);
    }

    public IQueryable<UserLevel>? FilterBy(Expression<Func<UserLevel, bool>> expression)
    {
        return _userLevels?.Where(expression);
    }

    public void Insert(UserLevel item)
    {
        _context.UserLevels.Add(item);
    }

    public void Update(UserLevel item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var userLevel = _context.UserLevels.Find(id);

        if(userLevel != null)
        {
            _context.UserLevels.Remove(userLevel);
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

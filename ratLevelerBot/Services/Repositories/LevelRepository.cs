using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services.Repositories;

public class LevelRepository : IRepository<Level>, IDisposable
{
    private readonly ApplicationDbContext _context;

    public LevelRepository(ApplicationDbContext context)
	{
        _context = context;
	}

    public IQueryable<Level> GetAll() 
    {
        return _context.Levels;   
    }

    public Level? GetById(long id) 
    {
        return _context.Levels.Find(id); 
    }

    public Level? FindBy(Expression<Func<Level, bool>> expression)
    {
        return _context.Levels.FirstOrDefault(expression);
    }

    public IQueryable<Level>? FilterBy(Expression<Func<Level, bool>> expression)
    {
        return _context.Levels.Where(expression);
    }

    public void Insert(Level item)
    {
        _context.Levels.Add(item);
    }

    public void Update(Level item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var level = _context.Levels.Find(id);

        if(level != null)
        {
            _context.Levels.Remove(level);
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


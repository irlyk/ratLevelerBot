using Microsoft.EntityFrameworkCore;
using RatLevelerBot.Models;

namespace RatLevelerBot.Services;

public class LevelRepository : ILevelRepository, IDisposable
{
    private ApplicationDbContext _context;

    public LevelRepository(ApplicationDbContext context)
	{
        _context = context;
	}

    public IQueryable<Level> GetAll() 
    {
        return _context.Levels;   
    }

    public Level? GetById(int levelId) 
    {
        return _context.Levels.Find(levelId); 
    }

    public void Insert(Level level)
    {
        _context.Levels.Add(level);
    }

    public void Update(Level level)
    {
        _context.Entry(level).State = EntityState.Modified;
    }

    public void Delete(int levelId)
    {
        var level = _context.Levels.Find(levelId);

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


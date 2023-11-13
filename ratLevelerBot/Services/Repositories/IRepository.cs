using System.Linq.Expressions;

namespace RatLevelerBot.Services.Repositories;

public interface IRepository<TItem> : IDisposable 
    where TItem : class  
{
        IQueryable<TItem> GetAll();

        TItem? GetById(long id);
        
        public TItem? FindBy(Expression<Func<TItem, bool>> expression);

        public IQueryable<TItem>? FilterBy(Expression<Func<TItem, bool>> expression);

        void Insert(TItem item);

        void Update(TItem item);

        void Delete(long id);

        void Save();
}

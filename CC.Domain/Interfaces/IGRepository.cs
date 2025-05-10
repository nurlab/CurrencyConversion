using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CC.Domain.Interfaces
{
    public interface IGRepository<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T t);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        int Count();
        Task<int> CountAsync();
        void Dispose(bool disposing);
        T Find(Func<T, bool> where);
        T Find(params object[] keys);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(params object[] keys);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> where);
        IQueryable<object> GetAll(Expression<Func<T, bool>> where, Expression<Func<T, object>> select);
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<IQueryable<object>> GetAllAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> select);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        T GetFirst();
        T GetFirst(Expression<Func<T, bool>> where);
        Task<T> GetFirstAsync();
        Task<T> GetFirstAsync(Expression<Func<T, bool>> where);
        T GetFirstOrDefault();
        T GetFirstOrDefault(Expression<Func<T, bool>> where);
        Task<T> GetFirstOrDefaultAsync();
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);
        T GetLast();
        T GetLast(Expression<Func<T, bool>> where);
        Task<T> GetLastAsync();
        Task<T> GetLastAsync(Expression<Func<T, bool>> where);
        T GetLastOrDefault();
        T GetLastOrDefault(Expression<Func<T, bool>> where);
        Task<T> GetLastOrDefaultAsync();
        Task<T> GetLastOrDefaultAsync(Expression<Func<T, bool>> where);
        T GetMax();
        object GetMax(Expression<Func<T, object>> selector);
        Task<T> GetMaxAsync();
        Task<object> GetMaxAsync(Expression<Func<T, object>> selector);
        T GetMin();
        object GetMin(Expression<Func<T, object>> selector);
        Task<T> GetMinAsync();
        Task<object> GetMinAsync(Expression<Func<T, object>> selector);
        EntityEntry<T> Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        EntityEntry<T> Update(T entity);
    }
}
using System.Linq.Expressions;
using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        DataContext GetContext();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        IQueryable<T> Where(Expression<Func<T, bool>> exp);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<T?> SingleOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> exp);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<int> DeleteRangeAsync(List<T> entities);
    }
}

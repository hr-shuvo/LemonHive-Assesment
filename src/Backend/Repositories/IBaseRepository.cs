using System.Linq.Expressions;

namespace Backend.Repositories;

public interface IBaseRepository<T>
{
    Task<PaginatedList<T>> LoadAsync(Expression<Func<T, bool>> filter, int page, int size);
    Task<T> GetByIdAsync(long id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(long id);
}
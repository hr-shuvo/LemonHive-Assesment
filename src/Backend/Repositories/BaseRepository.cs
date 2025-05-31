

using System.Linq.Expressions;
using Backend.Data;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _dbSet = context.Set<T>();
    }


    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Entity with id {id} not found.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(long id)
    {
        var entity = await _dbSet.FindAsync(id);

        return entity;
    }

    public async Task<PaginatedList<T>> LoadAsync(Expression<Func<T, bool>> filter, int page, int size)
    {
        if (page <= 0) page = 1;
        if (size <= 0) size = 10;

        var query = _dbSet.Where(filter);
        var count = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var result = new PaginatedList<T>(items, count, page, size);

        return result;
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}
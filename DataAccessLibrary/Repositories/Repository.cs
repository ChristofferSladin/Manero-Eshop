using DataAccessLibrary.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DataAccessLibrary.Repositories;

public abstract class Repository<TEntity> where TEntity : class
{
    private readonly ManeroDbContext _context;

    protected Repository(ManeroDbContext context)
    {
        _context = context;
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
            return entity ?? null!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return false;
    }
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            return await _context.Set<TEntity>().AnyAsync(expression);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return false;
    }
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null)
    {
        try
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = query.Where(expression);
            if (include != null)
            {
                query = include(query);
            }
            var entity = await query.FirstOrDefaultAsync();
            
            return entity ?? null!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression1, Expression<Func<TEntity, bool>> expression2)
    {
        try
        {
            var entity = await _context.Set<TEntity>().Where(expression1).Where(expression2).FirstOrDefaultAsync();
            return entity ?? null!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }
}

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FIREE.Db.Generic;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected DbContext Context;
    protected DbSet<TEntity> DbSet;

    protected GenericRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>?> FindAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<TEntity?> FindFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (orderBy != null)
        {
            return await orderBy(query).FirstAsync();
        }
        else
        {
            return await query.FirstAsync();
        }

    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await SaveAsync();
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }

    public virtual async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
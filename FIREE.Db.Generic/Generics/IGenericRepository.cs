using System.Linq.Expressions;

namespace FIREE.Db.Generic;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task InsertAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> FindFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        string includeProperties = "");
    Task<List<TEntity>?> FindAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        string includeProperties = "");
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteByIdAsync(int id);
    Task SaveAsync();
}

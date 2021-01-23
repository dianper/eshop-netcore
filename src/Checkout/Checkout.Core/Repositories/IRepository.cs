namespace Checkout.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Checkout.Core.Entities;

    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}

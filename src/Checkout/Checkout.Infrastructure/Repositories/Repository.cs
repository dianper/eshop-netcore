namespace Checkout.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Checkout.Core.Entities;
    using Checkout.Core.Repositories;
    using Checkout.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly OrderContext orderContext;

        public Repository(OrderContext orderContext)
        {
            this.orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            this.orderContext.Set<TEntity>().Add(entity);
            await this.orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this.orderContext.Set<TEntity>().Remove(entity);
            await this.orderContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await this.orderContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.orderContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await this.orderContext.Set<TEntity>().FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this.orderContext.Entry(entity).State = EntityState.Modified;
            await this.orderContext.SaveChangesAsync();
        }
    }
}

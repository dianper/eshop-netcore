namespace Identity.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Identity.Core.Entities;
    using Identity.Core.Repositories;
    using Identity.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AuthContext authContext;

        public Repository(AuthContext authContext)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            this.authContext.Set<TEntity>().Add(entity);
            await this.authContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this.authContext.Set<TEntity>().Remove(entity);
            await this.authContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync()
        {
            return await this.authContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.authContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await this.authContext.Set<TEntity>().FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this.authContext.Entry(entity).State = EntityState.Modified;
            await this.authContext.SaveChangesAsync();
        }
    }
}

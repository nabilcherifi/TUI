namespace Tui.Flights.Web.Infrastructure.DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using Tui.Flights.Web.Core.Models;

    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="TEntity">TEntity</typeparam>
    public class GenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets context
        /// </summary>
        public TuiDbContext Context { get; }

        /// <summary>
        /// Gets dbSet
        /// </summary>
        public DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// GenericRepository
        /// </summary>
        /// <param name="context">context</param>
        public GenericRepository(TuiDbContext context)
        {
                this.Context = context ?? throw new ArgumentNullException(nameof(context));
                this.DbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Expression code <Func<TEntity, bool>> filter means the caller will provide a lambda expression based on the TEntity type, and
        /// this expression will return a Boolean value.
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="orderBy">orderBy</param>
        /// <param name="includeProperties">includeProperties</param>
        /// <returns>IEnumerable</returns>
        public virtual IEnumerable<TEntity> GetDataEntities(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>TEntity</returns>
        public virtual TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity">entity</param>
        public virtual void Insert(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        /// <summary>
        /// Delete the entity from entity Id
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(object id)
        {
            while (true)
            {
                var entityToDelete = this.DbSet.Find(id);
                id = entityToDelete;
            }
        }

        /// <summary>
        /// Delete entity from entity Instance
        /// </summary>
        /// <param name="entityToDelete">entityToDelete</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.DbSet.Attach(entityToDelete);
            }
            this.DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entityToUpdate">entityToUpdate</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            this.DbSet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

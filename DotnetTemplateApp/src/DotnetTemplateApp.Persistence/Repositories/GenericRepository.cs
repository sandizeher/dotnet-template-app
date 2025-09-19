using System.Linq.Expressions;
using DotnetTemplateApp.Domain.Interfaces.Persistence;
using DotnetTemplateApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DotnetTemplateApp.Persistence.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _context;
        internal DbSet<TEntity> _dbSet;
        private static readonly char[] separator = [','];

        protected GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task<TEntity?> GetSingle(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return query.FirstOrDefaultAsync();
            }
        }

        public Task<TResult?> GetSingle<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> select,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split
                (separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var subQuery = query.Where(filter).Select(select);

            if (orderBy != null)
            {
                return orderBy(subQuery).FirstOrDefaultAsync();
            }

            return subQuery.FirstOrDefaultAsync();
        }

        public Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false,
            int? limit = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (limit != null)
            {
                query = query.Take(limit.Value);
            }

            return query.ToListAsync();
        }
        
        public Task<List<TResult>> Get<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> select,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false,
            int? limit = null,
            int? skip = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split
                (separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var subQuery = query.Where(filter).Select(select);

            if (orderBy != null)
            {
                subQuery = orderBy(subQuery);
            }

            if (skip != null)
            {
                subQuery = subQuery.Skip(skip.Value);
            }

            if (limit != null)
            {
                subQuery = subQuery.Take(limit.Value);
            }

            return subQuery.ToListAsync();
        }

        public virtual Task<int> ExecuteUpdateAsync(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
        }

        public virtual Task<int> ExecuteDeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
            => _dbSet
                .AsQueryable()
                .Where(predicate)
                .ExecuteDeleteAsync(cancellationToken);

        public virtual ValueTask<TEntity?> GetById(object id)
        {
            return _dbSet.FindAsync(id);
        }

       
        public Task Commit()
        {
            return _context.SaveChangesAsync();
        }

        public void ExecuteFunction(string commandText, params object[] parameters)
        {
            _context.Database.ExecuteSqlRaw(commandText, parameters);
        }
    }
}

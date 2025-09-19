using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DotnetTemplateApp.Domain.Interfaces.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get a single entity that matches the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        Task<TEntity?> GetSingle(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false);
        /// <summary>
        /// Get the specific selected property or properties of a single entity that matches the filter
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        Task<TResult?> GetSingle<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> select,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false);

        Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false,
            int? limit = null);

        Task<List<TResult>> Get<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> select,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null!,
            string includeProperties = "",
            bool asNoTracking = false,
            int? limit = null,
            int? skip = null);

        /// <summary>
        /// ExecuteUpdateAsync wrapper that enables test mocking and setups
        /// </summary>
        /// <param name="predicate">Filtering predicate</param>
        /// <param name="setPropertyCalls">Update calls</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<int> ExecuteUpdateAsync(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
            CancellationToken cancellationToken = default);

        Task<int> ExecuteDeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        ValueTask<TEntity?> GetById(object id);

        Task Commit();
    }
}

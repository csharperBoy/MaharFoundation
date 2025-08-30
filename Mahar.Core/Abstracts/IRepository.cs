using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mahar.Core.Abstracts
{
    /// <summary>
    /// Generic repository abstraction for asynchronous data access operations.
    /// Implementations should provide persistence logic; this interface contains no implementation.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TId">The entity identifier type.</typeparam>
    public interface IRepository<T, TId>
        where T : class, IEntity<TId>
    {
        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The entity if found; otherwise, implementation-specific behavior (e.g. null or exception).</returns>
        Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all entities.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A read-only list of all entities.</returns>
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The added entity (implementation may populate Id and audit fields).</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether any entity satisfies the given predicate.
        /// </summary>
        /// <param name="predicate">A LINQ expression used to test entities.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if any entity matches the predicate; otherwise false.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}

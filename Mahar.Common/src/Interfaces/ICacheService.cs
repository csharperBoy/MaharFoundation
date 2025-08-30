using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mahar.Common.Interfaces
{
    /// <summary>
    /// Abstraction for a simple caching service.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets an item from the cache or creates it using the provided factory.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="createItem">Factory method used to create the item if it's not present.</param>
        /// <param name="expiration">Optional expiration timespan.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
    Task<T> GetOrCreateAsync<T>(string cacheKey, Func<CancellationToken, Task<T>> createItem, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets an item in the cache.
        /// </summary>
        Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);
    }
}

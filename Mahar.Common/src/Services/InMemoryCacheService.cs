using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mahar.Common.Interfaces;

namespace Mahar.Common.Services
{
    /// <summary>
    /// In-memory cache implementation of <see cref="ICacheService"/> using <see cref="IMemoryCache"/>.
    /// </summary>
    public sealed class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<InMemoryCacheService> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="InMemoryCacheService"/>.
        /// </summary>
        public InMemoryCacheService(IMemoryCache cache, ILogger<InMemoryCacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<CancellationToken, Task<T>> createItem, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (cacheKey is null) throw new ArgumentNullException(nameof(cacheKey));
            if (createItem is null) throw new ArgumentNullException(nameof(createItem));

            try
            {
                if (_cache.TryGetValue(cacheKey, out T? existing))
                {
                    return existing!;
                }

                cancellationToken.ThrowIfCancellationRequested();
                var value = await createItem(cancellationToken).ConfigureAwait(false);
                var options = new MemoryCacheEntryOptions();
                if (expiration.HasValue) options.SetAbsoluteExpiration(expiration.Value);
                _cache.Set(cacheKey, value, options);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrCreateAsync for key {CacheKey}", cacheKey);
                throw;
            }
        }

        public Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
        {
            if (cacheKey is null) throw new ArgumentNullException(nameof(cacheKey));

            try
            {
                if (_cache.TryGetValue(cacheKey, out T? existing))
                {
                    return Task.FromResult(existing!);
                }

                return Task.FromResult(default(T)!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAsync for key {CacheKey}", cacheKey);
                throw;
            }
        }

        public Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (cacheKey is null) throw new ArgumentNullException(nameof(cacheKey));

            try
            {
                var options = new MemoryCacheEntryOptions();
                if (expiration.HasValue) options.SetAbsoluteExpiration(expiration.Value);
                _cache.Set(cacheKey, value, options);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetAsync for key {CacheKey}", cacheKey);
                throw;
            }
        }

        public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            if (cacheKey is null) throw new ArgumentNullException(nameof(cacheKey));

            try
            {
                _cache.Remove(cacheKey);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveAsync for key {CacheKey}", cacheKey);
                throw;
            }
        }
    }
}

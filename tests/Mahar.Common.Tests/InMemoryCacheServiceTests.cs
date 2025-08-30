using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Mahar.Common.Services;
using Xunit;

namespace Mahar.Common.Tests
{
    public class InMemoryCacheServiceTests
    {
        [Fact]
        public async Task GetOrCreateAsync_CacheMiss_CreatesValue()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new InMemoryCacheService(cache, new NullLogger<InMemoryCacheService>());

            var result = await service.GetOrCreateAsync("key", ct => Task.FromResult("value"), TimeSpan.FromMinutes(1));
            Assert.Equal("value", result);
        }

        [Fact]
        public async Task GetOrCreateAsync_Cancellation_Throws()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new InMemoryCacheService(cache, new NullLogger<InMemoryCacheService>());

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await service.GetOrCreateAsync("key", async ct => { await Task.Delay(100, ct); return "v"; }, TimeSpan.FromMinutes(1), cts.Token);
            });
        }
    }
}

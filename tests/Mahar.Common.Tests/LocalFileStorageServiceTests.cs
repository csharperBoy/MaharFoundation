using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Mahar.Common.Models;
using Mahar.Common.Services;
using Xunit;

namespace Mahar.Common.Tests
{
    public class LocalFileStorageServiceTests : IDisposable
    {
        private readonly string _tempDir;

        public LocalFileStorageServiceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try { Directory.Delete(_tempDir, true); } catch { }
        }

        [Fact]
        public async Task UploadDownloadDeleteFlow_Works()
        {
            var options = Options.Create(new LocalStorageOptions { BasePath = _tempDir, MaxFileSize = 1024 * 1024, AllowedExtensions = new[] { ".txt" } });
            var service = new LocalFileStorageService(options, new NullLogger<LocalFileStorageService>());

            var container = "c1";
            var fileName = "test.txt";
            var data = System.Text.Encoding.UTF8.GetBytes("hello");

            await using var ms = new MemoryStream(data);
            var result = await service.UploadFileAsync(ms, fileName, container);

            Assert.True(File.Exists(result.FilePath));

            // Ensure the downloaded stream is disposed before attempting to delete the file
            {
                await using var downloaded = await service.DownloadFileAsync(result.FilePath);
                using var sr = new StreamReader(downloaded);
                var text = await sr.ReadToEndAsync();
                Assert.Equal("hello", text);
            }

            await service.DeleteFileAsync(result.FilePath);
            Assert.False(File.Exists(result.FilePath));
        }
    }
}

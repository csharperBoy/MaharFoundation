using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mahar.Common.Interfaces;
using Mahar.Common.Models;

namespace Mahar.Common.Services
{
    /// <summary>
    /// Simple local file system storage implementation of <see cref="IFileStorageService"/>.
    /// </summary>
    public sealed class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly ILogger<LocalFileStorageService> _logger;

        private readonly LocalStorageOptions _options;

        public LocalFileStorageService(IOptions<LocalStorageOptions> options, ILogger<LocalFileStorageService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _basePath = !string.IsNullOrWhiteSpace(_options.BasePath)
                ? _options.BasePath
                : Path.Combine(Directory.GetCurrentDirectory(), "storage");
        }

        public async Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            if (fileStream is null) throw new ArgumentNullException(nameof(fileStream));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("fileName is required", nameof(fileName));
            if (string.IsNullOrWhiteSpace(containerName)) throw new ArgumentException("containerName is required", nameof(containerName));

            try
            {
                var containerPath = Path.Combine(_basePath, containerName);
                Directory.CreateDirectory(containerPath);

                var filePath = Path.Combine(containerPath, fileName);

                // Validate size if possible
                if (_options.MaxFileSize > 0 && fileStream.CanSeek)
                {
                    if (fileStream.Length > _options.MaxFileSize)
                        throw new ArgumentOutOfRangeException(nameof(fileStream), "File exceeds maximum allowed size.");
                    fileStream.Seek(0, SeekOrigin.Begin);
                }

                var ext = Path.GetExtension(fileName);
                if (_options.AllowedExtensions is { Length: > 0 } allowed && !string.IsNullOrWhiteSpace(ext))
                {
                    var ok = false;
                    foreach (var a in allowed)
                    {
                        if (string.Equals(a, ext, StringComparison.OrdinalIgnoreCase)) { ok = true; break; }
                    }
                    if (!ok) throw new ArgumentException("File extension is not allowed.", nameof(fileName));
                }

                await using var output = File.Create(filePath);
                fileStream.Seek(0, SeekOrigin.Begin);
                await fileStream.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
                var info = new FileInfo(filePath);

                return new FileUploadResult(filePath, fileName, info.Length, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName} to container {Container}", fileName, containerName);
                throw;
            }
        }

        public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required", nameof(filePath));

            try
            {
                var stream = File.OpenRead(filePath);
                return Task.FromResult((Stream)stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download file {FilePath}", filePath);
                throw;
            }
        }

        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required", nameof(filePath));

            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file {FilePath}", filePath);
                throw;
            }
        }
    }
}

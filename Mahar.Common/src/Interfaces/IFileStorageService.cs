using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mahar.Common.Models;

namespace Mahar.Common.Interfaces
{
    /// <summary>
    /// Abstraction for file storage operations.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file and returns information about the uploaded file.
        /// </summary>
        Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, string containerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file as a read-only stream.
        /// </summary>
        Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}

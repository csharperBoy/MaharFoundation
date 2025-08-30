using System.ComponentModel.DataAnnotations;

namespace Mahar.Common.Models
{
    /// <summary>
    /// Options for local file storage.
    /// </summary>
    public sealed class LocalStorageOptions
    {
        [Required]
        public string BasePath { get; init; } = "storage";

        [Range(1, long.MaxValue)]
        public long MaxFileSize { get; init; } = 10485760;

        public string[]? AllowedExtensions { get; init; }
    }
}

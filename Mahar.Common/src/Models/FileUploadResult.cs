namespace Mahar.Common.Models
{
    /// <summary>
    /// Represents the result of a file upload operation.
    /// </summary>
    /// <param name="FilePath">Full path where the file was stored.</param>
    /// <param name="FileName">Stored file name.</param>
    /// <param name="FileSize">Size in bytes.</param>
    /// <param name="ContentType">MIME content type.</param>
    public sealed record FileUploadResult(string FilePath, string FileName, long FileSize, string ContentType);
}

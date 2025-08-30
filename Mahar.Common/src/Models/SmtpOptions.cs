using System.ComponentModel.DataAnnotations;

namespace Mahar.Common.Models
{
    /// <summary>
    /// SMTP options used to configure the email service.
    /// </summary>
    public sealed class SmtpOptions
    {
        [Required]
        public string Host { get; init; } = "localhost";

        [Range(1, 65535)]
        public int Port { get; init; } = 587;

        public bool UseSsl { get; init; } = true;

        public string? Username { get; init; }

        public string? Password { get; init; }

        [EmailAddress]
        public string FromAddress { get; init; } = "noreply@mahar.com";

        public string FromName { get; init; } = "Mahar System";
    }
}

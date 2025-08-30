using System;
using System.Threading;
using System.Threading.Tasks;
using MimeKit;

namespace Mahar.Common.Interfaces
{
    /// <summary>
    /// Abstraction for SMTP client operations to allow mocking and isolation from MailKit.
    /// </summary>
    public interface IEmailSmtpClient : IDisposable
    {
        Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken);
        Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
        Task SendAsync(MimeMessage message, CancellationToken cancellationToken);
        Task DisconnectAsync(bool quit, CancellationToken cancellationToken);
    }
}

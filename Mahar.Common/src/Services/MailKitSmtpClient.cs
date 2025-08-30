using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Mahar.Common.Interfaces;
using MimeKit;

namespace Mahar.Common.Services
{
    /// <summary>
    /// MailKit-based implementation of <see cref="IEmailSmtpClient"/>.
    /// </summary>
    public sealed class MailKitSmtpClient : IEmailSmtpClient
    {
        private readonly SmtpClient _client = new SmtpClient();

        public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken)
        {
            var secure = useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            return _client.ConnectAsync(host, port, secure, cancellationToken);
        }

        public Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
            => _client.AuthenticateAsync(username, password, cancellationToken);

        public Task SendAsync(MimeMessage message, CancellationToken cancellationToken)
            => _client.SendAsync(message, cancellationToken);

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken)
            => _client.DisconnectAsync(quit, cancellationToken);

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

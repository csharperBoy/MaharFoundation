using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Options;
using MimeKit;
using Mahar.Common.Interfaces;
using Mahar.Common.Models;

namespace Mahar.Common.Services
{
    /// <summary>
    /// Email sender implementation using MailKit for fully asynchronous, robust SMTP operations.
    /// </summary>
    public sealed class MailKitEmailService : IEmailService, IDisposable
    {
        private readonly SmtpOptions _options;
    private readonly ILogger<MailKitEmailService> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IEmailSmtpClientFactory _clientFactory;
        private bool _disposed;

        /// <summary>
        /// Creates a new <see cref="MailKitEmailService"/>.
        /// </summary>
    public MailKitEmailService(IOptions<SmtpOptions> options, IEmailSmtpClientFactory clientFactory, ILogger<MailKitEmailService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // Simple retry policy for transient failures
            _retryPolicy = Policy.Handle<Exception>(ex => !(ex is OperationCanceledException))
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4) }, (ex, ts) =>
                {
                    _logger.LogWarning(ex, "Transient error during email send, will retry after {Delay}", ts);
                });
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            cancellationToken.ThrowIfCancellationRequested();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
            message.To.Add(MailboxAddress.Parse(request.To));
            message.Subject = request.Subject ?? string.Empty;

            var bodyBuilder = new BodyBuilder { HtmlBody = request.IsHtml ? request.Body : null, TextBody = request.IsHtml ? null : request.Body };
            message.Body = bodyBuilder.ToMessageBody();

            // Create and use SmtpClient per-send to avoid long-lived state and ensure thread-safety
            try
            {
                await _retryPolicy.ExecuteAsync(async ct =>
                {
                    using var client = _clientFactory.Create();
                    await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl, ct).ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(_options.Username))
                    {
                        await client.AuthenticateAsync(_options.Username, _options.Password ?? string.Empty, ct).ConfigureAwait(false);
                    }

                    await client.SendAsync(message, ct).ConfigureAwait(false);
                    await client.DisconnectAsync(true, ct).ConfigureAwait(false);
                }, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Email send cancelled for {To}", request.To);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", request.To);
                throw;
            }
        }

        /// <summary>
        /// Dispose any resources if necessary.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}

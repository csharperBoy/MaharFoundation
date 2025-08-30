using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mahar.Common.Interfaces;
using Mahar.Common.Models;

namespace Mahar.Common.Services
{
    /// <summary>
    /// Basic SMTP implementation of <see cref="IEmailService"/>.
    /// This implementation is intentionally minimal and intended as a starting point.
    /// </summary>
    public sealed class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            try
            {
                var smtpHost = _configuration["Smtp:Host"] ?? "localhost";
                var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var p) ? p : 25;

                using var message = new MailMessage();
                message.To.Add(request.To);
                message.Subject = request.Subject;
                message.Body = request.Body;
                message.IsBodyHtml = request.IsHtml;

                using var client = new SmtpClient(smtpHost, smtpPort);
                await client.SendMailAsync(message).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", request?.To);
                throw;
            }
        }
    }
}

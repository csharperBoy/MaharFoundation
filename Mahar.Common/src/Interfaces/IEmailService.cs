using System.Threading;
using System.Threading.Tasks;
using Mahar.Common.Models;

namespace Mahar.Common.Interfaces
{
    /// <summary>
    /// Abstraction for sending email messages.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default);
    }
}

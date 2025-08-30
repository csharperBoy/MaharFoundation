using Mahar.Common.Interfaces;

namespace Mahar.Common.Services
{
    /// <summary>
    /// Factory that creates <see cref="MailKitSmtpClient"/> instances.
    /// </summary>
    public sealed class MailKitSmtpClientFactory : IEmailSmtpClientFactory
    {
        public IEmailSmtpClient Create() => new MailKitSmtpClient();
    }
}

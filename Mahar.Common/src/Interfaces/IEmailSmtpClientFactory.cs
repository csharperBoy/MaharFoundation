namespace Mahar.Common.Interfaces
{
    /// <summary>
    /// Factory to create <see cref="IEmailSmtpClient"/> instances.
    /// </summary>
    public interface IEmailSmtpClientFactory
    {
        IEmailSmtpClient Create();
    }
}

using System;

namespace Mahar.Common.Models
{
    /// <summary>
    /// Represents an email send request.
    /// </summary>
    /// <param name="To">Recipient address.</param>
    /// <param name="Subject">Email subject.</param>
    /// <param name="Body">Email body.</param>
    /// <param name="IsHtml">Whether the body is HTML.</param>
    public sealed record EmailRequest(string To, string Subject, string Body, bool IsHtml = true);
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Mahar.Common.Models;
using Mahar.Common.Services;
using Xunit;

namespace Mahar.Common.Tests
{
    public class MailKitEmailServiceTests
    {
        [Fact]
        public async Task SendEmailAsync_CanceledToken_Throws()
        {
            var opts = Options.Create(new SmtpOptions { Host = "localhost", Port = 25, UseSsl = false });
            var svc = new MailKitEmailService(opts, new NullLogger<MailKitEmailService>());

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await svc.SendEmailAsync(new Mahar.Common.Models.EmailRequest("a@b.com", "s", "b", false), cts.Token);
            });
        }
    }
}

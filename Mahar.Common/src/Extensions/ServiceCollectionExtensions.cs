using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Mahar.Common.Interfaces;
using Mahar.Common.Services;

namespace Mahar.Common.Extensions
{
    /// <summary>
    /// Extension methods for registering Mahar.Common services into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the common services into the provided service collection.
        /// </summary>
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            // Options with validation
            services.AddOptions<Mahar.Common.Models.SmtpOptions>()
                .Bind(configuration.GetSection("Smtp"))
                .ValidateDataAnnotations()
                .Validate(o => o.Port > 0 && o.Port <= 65535, "Invalid port number")
                .ValidateOnStart();

            services.AddOptions<Mahar.Common.Models.LocalStorageOptions>()
                .Bind(configuration.GetSection("LocalStorage"))
                .ValidateDataAnnotations()
                .Validate(o => o.MaxFileSize > 0, "MaxFileSize must be positive")
                .ValidateOnStart();

            // Services
            services.AddScoped<ICacheService, InMemoryCacheService>();
            services.AddScoped<IEmailSmtpClientFactory, MailKitSmtpClientFactory>();
            services.AddScoped<IEmailService, Mahar.Common.Services.MailKitEmailService>();
            services.AddScoped<IFileStorageService, Mahar.Common.Services.LocalFileStorageService>();
            return services;
        }
    }
}

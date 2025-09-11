using Microsoft.Extensions.Configuration;

namespace Mahar.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IdentitySettings GetIdentitySettings(this IConfiguration configuration)
        {
            var identitySettings = new IdentitySettings();
            configuration.GetSection("IdentitySettings").Bind(identitySettings);
            return identitySettings;
        }
    }
}
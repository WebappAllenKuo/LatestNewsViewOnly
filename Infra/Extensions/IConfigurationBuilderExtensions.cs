using Microsoft.Extensions.Configuration;

namespace Infra.Extensions
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder config)
        {
            config.AddJsonFile("appsettings.json", false, true);
#if DEBUG
            config.AddJsonFile("appsettings.Debug.json", optional: false);
#endif
            return config;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Repositories.Account;
using Repositories.News;

namespace Repositories
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AccountRepository>();
            services.AddScoped<NewsRepository>();
            services.AddScoped<AttachmentRepository>();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Account;
using Services.Infra;
using Services.News;
using Services.Options;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddScoped<AccountService>();
            services.AddScoped<NewsService>();
            services.AddScoped<OptionService>();
            services.AddScoped<MemoryCacheService>();
            services.AddScoped<HashService>();
        }
    }
}

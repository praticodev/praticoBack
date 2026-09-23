using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pratico.Worker.OutBox
{
    public static class OutBoxWorkerConfig
    {
        public static IServiceCollection AddOutBoxWorker(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OutBoxWorkerOptions>(configuration.GetSection("OutBoxWorker"));
            services.AddScoped<IOutBoxMessageQueue, OutBoxMessageQueue>();
            services.AddScoped<OutBoxProcessor>();
            services.AddHostedService<OutBoxWorker>();

            return services;
        }
    }
}

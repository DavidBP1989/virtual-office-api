using cobach_api.Infrastructure.Interfaces;
using cobach_api.Infrastructure.Services;

namespace cobach_api.Infrastructure.StartupConfiguration
{
    public static partial class StartupConfiguration
    {
        public static void RegisterCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }
    }
}

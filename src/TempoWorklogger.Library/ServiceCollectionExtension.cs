using Microsoft.Extensions.DependencyInjection;
using TempoWorklogger.Library.Service;

namespace TempoWorklogger.Library
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTempoWorkloggerLibrary(this IServiceCollection services)
        {
            services.AddScoped<IFileReaderService, ExcelReaderService>();
            services.AddScoped<ITempoService, TempoService>();

            services.AddSingleton<IStorageService, StorageService>();
            return services;
        }
    }
}

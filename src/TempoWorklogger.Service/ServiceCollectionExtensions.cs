using Microsoft.Extensions.DependencyInjection;
using TempoWorklogger.Contract.Services;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Service.UI;

namespace TempoWorklogger.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTempoWorkloggerLibrary(this IServiceCollection services)
        {
            //services.AddScoped<IFileReaderService, ExcelReaderService>();
            services.AddScoped<ITempoService, TempoService>();

            services.AddSingleton<IImportStateService, ImportStateService>();
            //services.AddSingleton<IStorageService, StorageService>();
            return services;
        }

    }
}

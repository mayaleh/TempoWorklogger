using Microsoft.Extensions.DependencyInjection;
using TempoWorklogger.Contract.Config;
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
            
            // sqlite db
            services.AddSingleton<IDbService>(s =>
            {
                var appSettings = s.GetRequiredService<IAppConfig>();
                return new DbService(appSettings.DatabaseFileName);
            });
            return services;
        }

    }
}

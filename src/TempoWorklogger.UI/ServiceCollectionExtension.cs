using Microsoft.Extensions.DependencyInjection;
using Radzen;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.UI
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddScoped<NotificationService>();
            services.AddScoped<IUINotificationService, UINotificationService>();
            return services;
        }
    }
}

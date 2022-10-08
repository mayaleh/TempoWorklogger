using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TempoWorklogger.CQRS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
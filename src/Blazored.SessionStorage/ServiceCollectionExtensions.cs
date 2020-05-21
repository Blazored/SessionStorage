using System;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.SessionStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services)
        {
            return services
                .AddScoped<ISessionStorageService, SessionStorageService>()
                .AddScoped<ISyncSessionStorageService, SessionStorageService>()
                .Configure<SessionStorageOptions>(configureOptions =>
                {
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }

        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services, Action<SessionStorageOptions> configure)
        {
            return services
                .AddScoped<ISessionStorageService, SessionStorageService>()
                .AddScoped<ISyncSessionStorageService, SessionStorageService>()
                .Configure<SessionStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}

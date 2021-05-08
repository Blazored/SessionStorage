using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.SessionStorage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services)
            => AddBlazoredSessionStorage(services, null);

        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services, Action<SessionStorageOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, BrowserStorageProvider>()
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

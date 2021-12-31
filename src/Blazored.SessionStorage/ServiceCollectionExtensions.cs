using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blazored.SessionStorage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services)
            => AddBlazoredSessionStorage(services, null);

        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services, Action<SessionStorageOptions> configure, ServiceLifetime servicesLifetime = ServiceLifetime.Scoped)
        {
            services.TryAdd(new ServiceDescriptor(typeof(IJsonSerializer), typeof(SystemTextJsonSerializer), servicesLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IStorageProvider), typeof(BrowserStorageProvider), servicesLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ISessionStorageService), typeof(SessionStorageService), servicesLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ISyncSessionStorageService), typeof(SessionStorageService), servicesLifetime));
            services.Configure<SessionStorageOptions>(configureOptions =>
            {
                configure?.Invoke(configureOptions);
                configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
            return services;
        }
    }
}

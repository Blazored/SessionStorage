using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.SessionStorage;
using Blazored.SessionStorage.Testing;

namespace Bunit
{
    [ExcludeFromCodeCoverage]
    public static class BUnitSessionStorageTestExtensions
    {
        public static ISessionStorageService AddBlazoredSessionStorage(this TestContextBase context)
            => AddBlazoredSessionStorage(context, null);

        public static ISessionStorageService AddBlazoredSessionStorage(this TestContextBase context, Action<SessionStorageOptions> configure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));      

            context.Services
                .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                .AddSingleton<IStorageProvider, InMemoryStorageProvider>()
                .AddSingleton<ISessionStorageService, SessionStorageService>()
                .AddSingleton<ISyncSessionStorageService, SessionStorageService>()
                .Configure<SessionStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });

            return context.Services.GetService<ISessionStorageService>();
        }
    }
}

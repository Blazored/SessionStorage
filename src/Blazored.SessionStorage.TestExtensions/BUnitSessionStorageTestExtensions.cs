using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.SessionStorage;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Blazored.SessionStorage.TestExtensions;
using Microsoft.Extensions.DependencyInjection;

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

            var sessionStorageOptions = new SessionStorageOptions();
            configure?.Invoke(sessionStorageOptions);
            sessionStorageOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());

            var localStorageService = new SessionStorageService(new InMemoryStorageProvider(), new SystemTextJsonSerializer(sessionStorageOptions));
            context.Services.AddSingleton<ISessionStorageService>(localStorageService);

            return localStorageService;
        }
    }
}

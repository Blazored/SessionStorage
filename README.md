[![Nuget version](https://img.shields.io/nuget/v/Blazored.SessionStorage.svg?logo=nuget)](https://www.nuget.org/packages/Blazored.SessionStorage/)
[![Nuget downloads](https://img.shields.io/nuget/dt/Blazored.SessionStorage?logo=nuget)](https://www.nuget.org/packages/Blazored.SessionStorage/)
![Build & Test Main](https://github.com/Blazored/SessionStorage/workflows/Build%20&%20Test%20Main/badge.svg)

# Blazored SessionStorage
Blazored SessionStorage is a library that provides access to the browsers session storage APIs for Blazor applications. An additional benefit of using this library is that it will handle serializing and deserializing values when saving or retrieving them.

## Breaking Change (v1 > v2)

### JsonSerializerOptions
From v4 onwards we use the default the `JsonSerializerOptions` for `System.Text.Json` instead of using custom ones. This will cause values saved to session storage with v3 to break things.
To retain the old settings use the following configuration when adding Blazored SessionStorage to the DI container:

```csharp
builder.Services.AddBlazoredSessionStorage(config => {
        config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.IgnoreNullValues = true;
        config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        config.JsonSerializerOptions.WriteIndented = false;
    }
);
```

### SetItem[Async] method now serializes string values
Prior to v2 we bypassed the serialization of string values as it seemed pointless as string can be stored directly. However, this led to some edge cases where nullable strings were being saved as the string `"null"`. Then when retrieved, instead of being null the value was `"null"`. By serializing strings this issue is taken care of. 
For those who wish to save raw string values, a new method `SetValueAsString[Async]` is available. This will save a string value without attempting to serialize it and will throw an exception if a null string is attempted to be saved.

## Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="Blazored.SessionStorage" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package Blazored.SessionStorage
```

If you're using Jetbrains Rider or Visual Studio you can also install via the built in NuGet package manager.

## Setup

You will need to register the session storage services with the service collection in your _Startup.cs_ file in Blazor Server.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredSessionStorage();
}
``` 

Or in your _Program.cs_ file in Blazor WebAssembly.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazoredSessionStorage();

    await builder.Build().RunAsync();
}
```

### Registering services as Singleton - Blazor WebAssembly **ONLY**
99% of developers will want to register Blazored SessionStorage using the method described above. However, in some very specific scenarios,
developers may have a need to register services as Singleton as apposed to Scoped. This is possible by using the following method:

```csharp
builder.Services.AddBlazoredSessionStorageAsSingleton();
```

**This method will not work with Blazor Server applications as Blazor's JS interop services are registered as Scoped and cannot be injected into Singletons.**


## Usage (Blazor WebAssembly)
To use Blazored.SessionStorage in Blazor WebAssembly, inject the `ISessionStorageService` per the example below.

```c#
@inject Blazored.SessionStorage.ISessionStorageService sessionStorage

@code {

    protected override async Task OnInitializedAsync()
    {
        await sessionStorage.SetItemAsync("name", "John Smith");
        var name = await sessionStorage.GetItemAsync<string>("name");
    }

}
```

With Blazor WebAssembly you also have the option of a synchronous API, if your use case requires it. You can swap the `ISessionStorageService` for `ISyncSessionStorageService` which allows you to avoid use of `async`/`await`. For either interface, the method names are the same.

```c#
@inject Blazored.SessionStorage.ISyncSessionStorageService sessionStorage

@code {

    protected override void OnInitialized()
    {
        sessionStorage.SetItem("name", "John Smith");
        var name = sessionStorage.GetItem<string>("name");
    }

}
```

## Usage (Blazor Server)

**NOTE:** Due to pre-rendering in Blazor Server you can't perform any JS interop until the `OnAfterRender` lifecycle method.

```c#
@inject Blazored.SessionStorage.ISessionStorageService sessionStorage

@code {

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await sessionStorage.SetItemAsync("name", "John Smith");
        var name = await sessionStorage.GetItemAsync<string>("name");
    }

}
```

The APIs available are:

- asynchronous via `ISessionStorageService`:
  - SetItemAsync()
  - SetItemAsStringAsync()
  - GetItemAsync()
  - GetItemAsStringAsync()
  - RemoveItemAsync()
  - RemoveItemsAsync()
  - ClearAsync()
  - LengthAsync()
  - KeyAsync()
  - KeysAsync()
  - ContainKeyAsync()
  
- synchronous via `ISyncSessionStorageService` (Synchronous methods are **only** available in Blazor WebAssembly):
  - SetItem()
  - SetItemAsString()
  - GetItem()
  - GetItemAsString()
  - RemoveItem()
  - RemovesItem()
  - Clear()
  - Length()
  - Key()
  - Keys()
  - ContainKey()

**Note:** Blazored.SessionStorage methods will handle the serialisation and de-serialisation of the data for you, the exception is the `GetItemAsString[Async]` method which will return the raw string value from session storage.

## Configuring JSON Serializer Options
You can configure the options for the default serializer (System.Text.Json) when calling the `AddBlazoredSessionStorage` method to register services.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazoredSessionStorage(config =>
        config.JsonSerializerOptions.WriteIndented = true
    );

    await builder.Build().RunAsync();
}
```

## Using a custom JSON serializer
By default, the library uses `System.Text.Json`. If you prefer to use a different JSON library for serialization--or if you want to add some custom logic when serializing or deserializing--you can provide your own serializer which implements the `Blazored.SessionStorage.Serialization.IJsonSerializer` interface.

To register your own serializer in place of the default one, you can do the following:

```csharp
builder.Services.AddBlazoredSessionStorage();
builder.Services.Replace(ServiceDescriptor.Scoped<IJsonSerializer, MySerializer>());
```

You can find an example of this in the Blazor Server sample project. The standard serializer has been replaced with a new serializer which uses NewtonsoftJson.

## Testing with bUnit
This library provides test extensions for use with the [bUnit testing library](https://bunit.dev/). Using these test extensions will provide an in memory implementation which mimics session storage allowing more realistic testing of your components.

### Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="Blazored.SessionStorage.TestExtensions" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package Blazored.SessionStorage.TestExtensions
```

If you're using Jetbrains Rider or Visual Studio you can also install via the built in NuGet package manager.

### Usage example

Below is an example test which uses these extensions. You can find an example project which shows this code in action in the samples folder.

```c#
public class IndexPageTests : TestContext
{
    [Fact]
    public async Task SavesNameToSessionStorage()
    {
        // Arrange
        const string inputName = "John Smith";
        var sessionStorage = this.AddBlazoredSessionStorage();
        var cut = RenderComponent<BlazorWebAssembly.Pages.Index>();

        // Act
        cut.Find("#Name").Change(inputName);
        cut.Find("#NameButton").Click();
            
        // Assert
        var name = await sessionStorage.GetItemAsync<string>("name");
            
        Assert.Equal(inputName, name);
    }
}
```

# Blazored SessionStorage
A library to provide access to session storage in Blazor applications

[![Build Status](https://dev.azure.com/blazored/SessionStorage/_apis/build/status/Blazored.SessionStorage?branchName=master)](https://dev.azure.com/blazored/SessionStorage/_build/latest?definitionId=1&branchName=master)

![Nuget](https://img.shields.io/nuget/v/blazored.sessionstorage.svg)

### Installing

You can install from NuGet using the following command:

`Install-Package Blazored.SessionStorage`

Or via the Visual Studio package manger.

### Setup

You will need to register the session storage services with the service collection in your _startup.cs_ file.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredSessionStorage();
}
``` 

### Usage (Blazor WebAssembly)
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

### Usage (Blazor Server)

**NOTE:** Due to pre-rendering in Blazor Server you can't perform any JS interop until the `OnAfterRender` lifecycle method.

```c#
@inject Blazored.SessionStorage.ISessionStorageService sessionStorage

@functions {

    protected override async Task OnAfterRenderAsync()
    {
        await sessionStorage.SetItemAsync("name", "John Smith");
        var name = await sessionStorage.GetItemAsync<string>("name");
    }

}
```

The APIs available are:

- asynchronous via `ISessionStorageService`:
  - SetItemAsync()
  - GetItemAsync()
  - RemoveItemAsync()
  - ClearAsync()
  - LengthAsync()
  - KeyAsync()
- synchronous via `ISyncSessionStorageService`:
  - SetItem()
  - GetItem()
  - RemoveItem()
  - Clear()
  - Length()
  - Key()

**Note:** Blazored.SessionStorage methods will handle the serialisation and de-serialisation of the data for you.

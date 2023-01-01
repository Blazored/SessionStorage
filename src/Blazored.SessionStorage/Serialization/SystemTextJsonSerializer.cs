using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Blazored.SessionStorage.Serialization;

internal class SystemTextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonSerializer(IOptions<SessionStorageOptions> options)
    {
        _options = options.Value.JsonSerializerOptions;
    }

    public SystemTextJsonSerializer(SessionStorageOptions sessionStorageOptions)
    {
        _options = sessionStorageOptions.JsonSerializerOptions;
    }

    public T Deserialize<T>(string data) 
        => JsonSerializer.Deserialize<T>(data, _options);

    public string Serialize<T>(T data)
        => JsonSerializer.Serialize(data, _options);
}
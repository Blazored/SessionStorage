using System.Text.Json;

namespace Blazored.SessionStorage.StorageOptions;

public class SessionStorageOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}

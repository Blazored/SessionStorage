using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("Blazored.SessionStorage.Tests, PublicKey=" +
"00240000048000009400000006020000002400005253413100040000010001007527e122cc36dc" +
"13c695a4f43b7c2da3f631aed456ed309140c0d52262323e25d84bef7feddc8bd29cfe46ec6521" +
"86cb10e059eedabf2ff000b977a2376a613dccfb092de6c243e0888db4c66a084124b2c1799bda" +
"4bbb2f70fed0382fc1cbdafa6dc0f4baccdc2cee55234f8a5ad76645c315523fee5352d9f01036" +
"e48b13e3")]
namespace Blazored.SessionStorage.Serialization
{
    internal class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<SessionStorageOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T Deserialize<T>(string data) 
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}

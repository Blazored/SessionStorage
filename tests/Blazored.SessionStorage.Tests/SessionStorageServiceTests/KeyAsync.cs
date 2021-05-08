using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Blazored.SessionStorage.Testing;
using Blazored.SessionStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.SessionStorage.Tests.SessionStorageServiceTests
{
    public class KeyAsync
    {
        private readonly SessionStorageService _sut;

        public KeyAsync()
        {
            var mockOptions = new Mock<IOptions<SessionStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new SessionStorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            IStorageProvider storageProvider = new InMemoryStorageProvider();
            _sut = new SessionStorageService(storageProvider, serializer);
        }

        [Fact]
        public async Task ReturnsNameOfKeyAtIndex_When_KeyExistsInStore()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            await _sut.SetItemAsync(key1, item1);
            await _sut.SetItemAsync(key2, item2);
            
            // Act
            var keyName = await _sut.KeyAsync(1);

            // Assert
            Assert.Equal(key2, keyName);
        }

        [Fact]
        public async Task ReturnsNull_When_KeyDoesNotExistInStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            await _sut.SetItemAsync("TestKey1", item1);
            
            // Act
            var keyName = await _sut.KeyAsync(1);

            // Assert
            Assert.Null(keyName);
        }
    }
}

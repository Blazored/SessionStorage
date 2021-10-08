using System.Text.Json;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Blazored.SessionStorage.TestExtensions;
using Blazored.SessionStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.SessionStorage.Tests.SessionStorageServiceTests
{
    public class Key
    {
        private readonly SessionStorageService _sut;

        public Key()
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
        public void ReturnsNameOfKeyAtIndex_When_KeyExistsInStore()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);
            
            // Act
            var keyName = _sut.Key(1);

            // Assert
            Assert.Equal(key2, keyName);
        }

        [Fact]
        public void ReturnsNull_When_KeyDoesNotExistInStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            _sut.SetItem("TestKey1", item1);
            
            // Act
            var keyName = _sut.Key(1);

            // Assert
            Assert.Null(keyName);
        }
    }
}

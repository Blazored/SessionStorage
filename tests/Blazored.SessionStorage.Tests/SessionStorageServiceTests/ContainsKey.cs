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
    public class ContainsKey
    {
        private readonly SessionStorageService _sut;

        public ContainsKey()
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
        public void ReturnsTrue_When_KeyExistsInStore()
        {
            // Arrange
            const string key = "TestKey";
            var item1 = new TestObject(1, "Jane Smith");
            _sut.SetItem(key, item1);
            
            // Act
            var containsKey = _sut.ContainKey(key);

            // Assert
            Assert.True(containsKey);
        }

        [Fact]
        public void ReturnsFalse_When_KeyDoesNotExistsInStore()
        {
            // Arrange
            const string key = "TestKey";
            
            // Act
            var containsKey = _sut.ContainKey(key);

            // Assert
            Assert.False(containsKey);
        }
    }
}

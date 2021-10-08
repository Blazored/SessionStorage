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
    public class Clear
    {
        private readonly SessionStorageService _sut;
        private readonly IStorageProvider _storageProvider;

        public Clear()
        {
            var mockOptions = new Mock<IOptions<SessionStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new SessionStorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new SessionStorageService(_storageProvider, serializer);
        }

        [Fact]
        public void ClearsAnyItemsInTheStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem("Item1", item1);
            _sut.SetItem("Item2", item2);

            // Act
            _sut.Clear();

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
        
        [Fact]
        public void DoesNothingWhenStoreIsEmpty()
        {
            // Act
            _sut.Clear();

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
    }
}

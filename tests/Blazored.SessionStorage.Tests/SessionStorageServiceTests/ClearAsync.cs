using System.Text.Json;
using System.Threading.Tasks;
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
    public class ClearAsync
    {
        private readonly SessionStorageService _sut;
        private readonly IStorageProvider _storageProvider;

        public ClearAsync()
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
        public async Task ClearsAnyItemsInTheStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            await _sut.SetItemAsync("Item1", item1);
            await _sut.SetItemAsync("Item2", item2);

            // Act
            await _sut.ClearAsync();

            // Assert
            Assert.Equal(0, await _storageProvider.LengthAsync());
        }
        
        [Fact]
        public async Task DoesNothingWhenItemDoesNotExistInStore()
        {
            // Act
            await _sut.ClearAsync();

            // Assert
            Assert.Equal(0, await _storageProvider.LengthAsync());
        }
    }
}

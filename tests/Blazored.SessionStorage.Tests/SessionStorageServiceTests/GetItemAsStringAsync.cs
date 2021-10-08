using System;
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
    public class GetItemAsStringAsync
    {
        private readonly SessionStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private const string Key = "testKey";

        public GetItemAsStringAsync()
        {
            var mockOptions = new Mock<IOptions<SessionStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new SessionStorageOptions());
            _serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new SessionStorageService(_storageProvider, _serializer);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ThrowsArgumentNullException_When_KeyIsInvalid(string key)
        {
            // arrange / act
            var action = new Func<Task>(async () => await _sut.GetItemAsStringAsync(key));

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(action);
        }
        
        [Theory]
        [InlineData("Item1", "stringTest")]
        [InlineData("Item2", 11)]
        [InlineData("Item3", 11.11)]
        public async Task ReturnsDeserializedDataFromStore<T>(string key, T data)
        {
            // Arrange
            await _sut.SetItemAsync(key, data);
            
            // Act
            var result = await _sut.GetItemAsStringAsync(key);

            // Assert
            var serializedData = _serializer.Serialize(data);
            Assert.Equal(serializedData, result);
        }

        [Fact]
        public async Task ReturnsComplexObjectFromStore()
        {
            // Arrange
            var objectToSave = new TestObject(2, "Jane Smith");
            await _sut.SetItemAsync(Key, objectToSave);

            // Act
            var result = await _sut.GetItemAsStringAsync(Key);

            // Assert
            var serializedData = _serializer.Serialize(objectToSave);
            Assert.Equal(serializedData, result);
        }
    }
}

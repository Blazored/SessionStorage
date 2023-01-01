using System.Linq;
using System.Text.Json;
using Blazored.SessionStorage.JsonConverters;
using Blazored.SessionStorage.Serialization;
using Blazored.SessionStorage.StorageOptions;
using Blazored.SessionStorage.TestExtensions;
using Blazored.SessionStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.SessionStorage.Tests.SessionStorageServiceTests;

public class Keys
    {
        private readonly SessionStorageService _sut;

        public Keys()
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
        public void ReturnsKeysAsync()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            _sut.Clear();
            
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);

            // Act
            var keyNames = _sut.KeysAsync().Result.ToList();

            // Assert
            Assert.Collection(keyNames, 
                                item => Assert.Equal(key1, item),
                                item => Assert.Equal(key2, item));
        }

        [Fact]
        public void ReturnsKeys()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            _sut.Clear();

            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");

            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);

            // Act
            var keyNames = _sut.Keys().ToList();

            // Assert
            Assert.Collection(keyNames,
                                item => Assert.Equal(key1, item),
                                item => Assert.Equal(key2, item));
        }

        [Fact]
        public void ReturnsEmptyWhenCollectionEmptyAsync()
        {
            // Arrange
            _sut.Clear();

            // Act
            var keyNames = _sut.KeysAsync().Result;

            // Assert
            Assert.Empty(keyNames);
        }

        [Fact]
        public void ReturnsEmptyWhenCollectionEmpty()
        {
            // Arrange
            _sut.Clear();

            // Act
            var keyNames = _sut.Keys();

            // Assert
            Assert.Empty(keyNames);
        }

        [Fact]
        public void RemoveKeysAsync()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            const string key3 = "TestKey3";
            _sut.Clear();

            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            var item3 = new TestObject(3, "Jade Smith");

            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);
            _sut.SetItem(key3, item3);

            // Act
            var keyNames = new[] { key1, key2 };
            _sut.RemoveItemsAsync(keyNames).AsTask().Wait();

            // Assert
            Assert.Equal(1, _sut.Length());
            Assert.NotNull(_sut.GetItem<TestObject>(key3));
        }

        [Fact]
        public void RemoveKeys()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            const string key3 = "TestKey3";
            _sut.Clear();

            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            var item3 = new TestObject(3, "Jade Smith");

            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);
            _sut.SetItem(key3, item3);

            // Act
            var keyNames = new[] { key1, key2 };
            _sut.RemoveItems(keyNames);

            // Assert
            Assert.Equal(1, _sut.Length());
            Assert.NotNull(_sut.GetItem<TestObject>(key3));
        }
    }

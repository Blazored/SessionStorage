using System;
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

public class RemoveItem
{
    private readonly SessionStorageService _sut;
    private readonly IStorageProvider _storageProvider;

    private const string Key = "testKey";

    public RemoveItem()
    {
        var mockOptions = new Mock<IOptions<SessionStorageOptions>>();
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new TimespanJsonConverter());
        mockOptions.Setup(u => u.Value).Returns(new SessionStorageOptions());
        IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
        _storageProvider = new InMemoryStorageProvider();
        _sut = new SessionStorageService(_storageProvider, serializer);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ThrowsArgumentNullException_When_KeyIsInvalid(string key)
    {
        // arrange / act
        var action = new Action(() => _sut.RemoveItem(key));

        // assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void RemovesItemFromStoreIfExists()
    {
        // Arrange
        var data = new TestObject(2, "Jane Smith");
        _sut.SetItem(Key, data);

        // Act
        _sut.RemoveItem(Key);

        // Assert
        Assert.Equal(0, _storageProvider.Length());
    }
        
    [Fact]
    public void DoesNothingWhenItemDoesNotExistInStore()
    {
        // Act
        _sut.RemoveItem(Key);

        // Assert
        Assert.Equal(0, _storageProvider.Length());
    }
}
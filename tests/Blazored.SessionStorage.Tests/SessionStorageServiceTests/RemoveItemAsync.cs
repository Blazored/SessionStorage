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

namespace Blazored.SessionStorage.Tests.SessionStorageServiceTests;

public class RemoveItemAsync
{
    private readonly SessionStorageService _sut;
    private readonly IStorageProvider _storageProvider;

    private const string Key = "testKey";

    public RemoveItemAsync()
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
        var action = new Func<Task>(async () => await _sut.RemoveItemAsync(key));

        // assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact]
    public async Task RemovesItemFromStoreIfExists()
    {
        // Arrange
        var data = new TestObject(2, "Jane Smith");
        await _sut.SetItemAsync(Key, data);

        // Act
        await _sut.RemoveItemAsync(Key);

        // Assert
        Assert.Equal(0, await _storageProvider.LengthAsync());
    }
        
    [Fact]
    public async Task DoesNothingWhenItemDoesNotExistInStore()
    {
        // Act
        await _sut.RemoveItemAsync(Key);

        // Assert
        Assert.Equal(0, await _storageProvider.LengthAsync());
    }
}
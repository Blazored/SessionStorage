using System.Threading.Tasks;
using Bunit;
using Xunit;

namespace bUnitExample
{
    public class IndexPageTests : TestContext
    {
        [Fact]
        public async Task SavesNameToSessionStorage()
        {
            // Arrange
            const string inputName = "John Smith";
            var sessionStorage = this.AddBlazoredSessionStorage();
            var cut = RenderComponent<BlazorWebAssembly.Pages.Index>();

            // Act
            cut.Find("#Name").Change(inputName);
            cut.Find("#NameButton").Click();
            
            // Assert
            var name = await sessionStorage.GetItemAsync<string>("name");
            
            Assert.Equal(inputName, name);
        }
    }
}

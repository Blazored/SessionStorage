using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorClientSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddBlazoredSessionStorage();
            await builder.Build().RunAsync();
        }
    }
}

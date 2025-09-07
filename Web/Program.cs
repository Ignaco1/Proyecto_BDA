using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using static System.Net.WebRequestMethods;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            var url = "https://localhost:7248";
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}

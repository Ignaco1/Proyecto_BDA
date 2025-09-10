using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Web.Auth;
using Web.Servicios;
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
            builder.Services.AddAuthorizationCore();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<ProveedorAutenticacionJWT>();
            builder.Services.AddScoped<ServicioAuth>();
            builder.Services.AddScoped<IServicioLogin, ProveedorAutenticacionJWT>(x => x.GetRequiredService<ProveedorAutenticacionJWT>());

            await builder.Build().RunAsync();
        }
    }
}

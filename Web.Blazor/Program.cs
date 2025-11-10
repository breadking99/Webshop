using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Interfaces;
using Web.Blazor;
using Web.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Inject HttpClients:
Uri baseAddress = new(builder.Configuration.GetValue<string>("BaseAddress")!);
TimeSpan timeout = TimeSpan.FromSeconds(30);
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = baseAddress,
    Timeout = timeout
});

// Add Services:
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();

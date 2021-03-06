using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Shared.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<SELearning.App>("#app");

builder.Services.AddHttpClient("SELearning.API",
        client => client.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "Api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SELearning.API"));

builder.Services.AddScoped<ToastService, ToastService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://340cf90c-0ea4-48a6-902c-b1ef13d801a8/API.Access");
    options.UserOptions.RoleClaim = "appRole";
});

await builder.Build().RunAsync();

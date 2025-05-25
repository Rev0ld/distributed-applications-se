using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using VideoLibraryBlazorFrontend.Components;

namespace VideoLibraryBlazorFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddDownstreamApi("MyApi", builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();

        builder.Services.AddAuthorization();

        builder.Services.AddHttpClient();

        builder.Services.AddScoped<ApiAuthorizationHandler>();

        builder.Services.AddHttpClient("AuthorizedApiClient", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["DownstreamApi:BaseUrl"]!);
        })
        .AddHttpMessageHandler<ApiAuthorizationHandler>();

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedApiClient"));


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.MapGroup("/authentication").MapLoginAndLogout();
        app.Run();
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace DevelopmentAspNetCoreKeyVault;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        var client = new SecretClient(new Uri(builder.Configuration["AzureKeyVaultEndpoint"]!), 
            new DefaultAzureCredential());

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}

namespace DevelopmentAspNetCoreKeyVault;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        var keyVault = builder.Configuration["AzureKeyVaultEndpoint"];
        if(!string.IsNullOrEmpty(keyVault))
        {
            builder.Configuration.AddAzureKeyVault(
            new Uri($"{builder.Configuration["AzureKeyVaultEndpoint"]}"),
            AppAccessCredentials.GetChainedTokenCredentials(builder.Configuration,
                builder.Environment.IsDevelopment()));
        }
        
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

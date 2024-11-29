using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevelopmentAspNetCoreKeyVault.Pages;

public class IndexModel(IConfiguration configuration, IHostEnvironment hostEnvironment) : PageModel
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

    [BindProperty]
    public string? DemoSecret { get; set; }

    [BindProperty]
    public string? DemoSecretConfig { get; set; }

    public async Task OnGetAsync()
    {
        // Azure SDK direct
        var client = new SecretClient(new Uri(_configuration["AzureKeyVaultEndpoint"]!),
            AppAccessCredentials.GetChainedTokenCredentials(_configuration, 
                _hostEnvironment.IsDevelopment()));

        var secret = await client.GetSecretAsync("demosecret");
        DemoSecret = secret!.Value.Value;

        // #########################

        // ASP.NET Core configuration
        // From from key vault using ASP.NET Core configuration integration
        DemoSecretConfig = _configuration["demosecret"];

        
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevelopmentAspNetCoreKeyVault.Pages;

public class IndexModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;

    [BindProperty]
    public string? DemoSecret { get; set; }  
    
    public async Task OnGetAsync()
    {
        // aspnetcore-keyvault
        var client = new SecretClient(new Uri(_configuration["AzureKeyVaultEndpoint"]!),
            new DefaultAzureCredential());

        var secret = await client.GetSecretAsync("demosecret");
        DemoSecret = secret!.Value.Value;
    }
}

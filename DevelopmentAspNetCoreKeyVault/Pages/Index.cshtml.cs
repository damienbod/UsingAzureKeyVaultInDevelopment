using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevelopmentAspNetCoreKeyVault.Pages;

public class IndexModel(IConfiguration configuration, KeyVaultCredentialsClient keyVaultCredentialsClient) : PageModel
{
    private readonly IConfiguration _configuration = configuration;
    private readonly KeyVaultCredentialsClient _KeyVaultCredentialsClient = keyVaultCredentialsClient;

    [BindProperty]
    public string? DemoSecret { get; set; }  
    
    public async Task OnGetAsync()
    {
        // aspnetcore-keyvault
        var client = new SecretClient(new Uri(_configuration["AzureKeyVaultEndpoint"]!),
            _KeyVaultCredentialsClient.GetChainedTokenCredentials());

        var secret = await client.GetSecretAsync("demosecret");
        DemoSecret = secret!.Value.Value;
    }
}

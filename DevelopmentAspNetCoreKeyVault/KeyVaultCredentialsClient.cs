using Azure.Identity;

namespace DevelopmentAspNetCoreKeyVault;

public class KeyVaultCredentialsClient
{

    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public KeyVaultCredentialsClient(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public ChainedTokenCredential GetChainedTokenCredentials()
    {
        if (!_environment.IsDevelopment())
        {
            return new ChainedTokenCredential(new ManagedIdentityCredential());
        }
        else // dev env
        {
            var tenantId = _configuration["EntraId:TenantId"];
            var clientId = _configuration.GetValue<string>("EntraId:ClientId");
            var clientSecret = _configuration.GetValue<string>("EntraId:ClientSecret");

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var devClientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var chainedTokenCredential = new ChainedTokenCredential(devClientSecretCredential);

            return chainedTokenCredential;
        }
    }
}
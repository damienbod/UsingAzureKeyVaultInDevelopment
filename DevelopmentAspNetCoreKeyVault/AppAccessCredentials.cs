using Azure.Identity;

namespace DevelopmentAspNetCoreKeyVault;

public static class AppAccessCredentials
{
    public static ChainedTokenCredential GetChainedTokenCredentials(IConfiguration configuration, bool isDevelopment)
    {
        if (!isDevelopment)
        {
            // Use a system assigned managed identity on production deployments
            return new ChainedTokenCredential(new ManagedIdentityCredential());
        }
        else // dev env
        {
            var tenantId = configuration.GetValue<string>("EntraId:TenantId", string.Empty);
            var clientId = configuration.GetValue<string>("EntraId:ClientId", string.Empty);
            var clientSecret = configuration.GetValue<string>("EntraId:ClientSecret", string.Empty);

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var devClientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var chainedTokenCredential = new ChainedTokenCredential(devClientSecretCredential, new ManagedIdentityCredential());

            return chainedTokenCredential;
        }
    }
}

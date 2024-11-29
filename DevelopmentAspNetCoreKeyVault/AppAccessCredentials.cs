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
            var tenantId = configuration["EntraId:TenantId"];
            var clientId = configuration.GetValue<string>("EntraId:ClientId");
            var clientSecret = configuration.GetValue<string>("EntraId:ClientSecret");

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
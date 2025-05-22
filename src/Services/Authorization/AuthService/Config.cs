using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.DataProtection;

namespace AuthService
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope("packages.fullaccess", "Full access to Packages API"),
            new ApiScope("delivery.fullaccess", "Full access to delivery API"),
    };

        public static IEnumerable<Client> Clients => new List<Client>
    {
        new Client
        {
            ClientId = "packagesclient",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Duende.IdentityServer.Models.Secret("secret".Sha256()) },
            AllowedScopes = { "packages.fullaccess" }
        },
        new Client
        {
            ClientId = "deliveryclient",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Duende.IdentityServer.Models.Secret("secret".Sha256()) },
            AllowedScopes = { "delivery.fullaccess" }
        }
    };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
    {
        new ApiResource("packagesapi", "Packages API")
        {
            Scopes = { "packages.fullaccess" }
        },
        new ApiResource("deliveryapi", "Delivery API")
        {
            Scopes = { "delivery.fullaccess" }
        }
    };
    }
}

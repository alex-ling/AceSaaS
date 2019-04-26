using IdentityServer4.Models;
using System.Collections.Generic;

namespace Acesoft.Rbac.Config
{
    public class IS4Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "WebApi")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "aceclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("Us1e3cv5rsMet4pv".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api"
                    },
                    AccessTokenLifetime = 1296000,
                    AllowOfflineAccess = true
                }
            };
        }
    }
}

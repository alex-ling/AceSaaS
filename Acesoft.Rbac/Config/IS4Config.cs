using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

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
                        "api", StandardScopes.OfflineAccess
                    },
                    AccessTokenLifetime = 2592000,
                    AbsoluteRefreshTokenLifetime = 2592000,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AllowOfflineAccess = true
                }
            };
        }
    }
}

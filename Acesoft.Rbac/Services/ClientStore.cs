using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;
using Acesoft.Rbac.Config;

namespace Acesoft.Rbac.Services
{
    public class ClientStore : IClientStore
    {
        public ClientStore()
        {
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(IS4Config.GetClients().Single((Client c) => c.ClientId == clientId));
        }
    }
}

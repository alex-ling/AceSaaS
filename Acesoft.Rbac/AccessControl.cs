using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Acesoft.Logger;
using Acesoft.Rbac.Entity;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace Acesoft.Rbac
{
    public class AccessControl : IAccessControl
    {
        private readonly ILogger<AccessControl> logger = LoggerContext.GetLogger<AccessControl>();

        private readonly ConcurrentDictionary<string, Func<object>> stateResolvers;
        private IEnumerable<IStateProvider> stateProviders;

        private IHttpContextAccessor httpContextAccessor;
        private IUserService userService;
        private Rbac_User user;

        public HttpContext HttpContext => httpContextAccessor.HttpContext;

        public bool Logined => User.LoginName != "guest";
        public bool IsRoot => User.LoginName == "root";
        public bool IsAdmin => User.LoginName == "admin";
        public string InRoles => $"({User.Rbac_UAs.Join(ua => ua.Role_Id)})";
        public IEnumerable<long> Roles => User.Rbac_UAs.Select(ua => ua.Role_Id);
        public IDictionary<string, string> Params => User.Rbac_Params.ToDictionary(p => p.Name, p => p.Value);
        public IDictionary<string, string> Auths => User.Rbac_Auths.ToDictionary(a => a.AuthType, a => a.AuthId);

        public Rbac_User User
        {
            get
            {
                if (user == null)
                {
                    user = Get<Rbac_User>("CurrentUser");
                }
                return user;
            }
        }

        public AccessControl(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;

            this.stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        }

        public Task Login(string userName, string password, bool persistent)
        {
            return Login(userService.Login(userName, password), persistent);
        }

        public Task Login(long appId, string authId, bool persistent)
        {
            return Login(userService.QueryByAuth(appId, authId), persistent);
        }

        public Task Login(Rbac_User user, bool persistent)
        {
            this.user = user;

            logger.LogDebug($"Login with the user \"{user.Id}:{user.LoginName}\"");
            var ticket = Membership.AuthenticationTicket(user.HashId, persistent);
            return HttpContext.SignInAsync(ticket.AuthenticationScheme, ticket.Principal, ticket.Properties);
        }

        public void UpdateUser(long appId, string authId, string authType, bool needSaveUser)
        {
            if (needSaveUser)
            {
                userService.Update(user);
            }

            var auth = userService.UpdateAuth(user.Id, appId, authId, authType);
            if (!Auths.ContainsKey(authType))
            {
                user.Rbac_Auths.Add(auth);
            }
        }

        public Task Logout()
        {
            user = null;
            return HttpContext.SignOutAsync(Membership.Auth_Cookie);
        }

        public bool IsInRole(long roleId) => Roles.Contains(roleId);

        public bool CheckAccess(long refId)
        {
            throw new NotImplementedException();
        }

        #region state
        public T Get<T>(string name)
        {
            var state = stateResolvers.GetOrAdd(name, key => FindResolverForState<T>(key));
            if (state != null)
            {
                return (T)state();
            }
            return default(T);
        }

        public void Set(string name, object value)
        {
            if (stateResolvers.ContainsKey(name))
            {
                stateResolvers[name] = () => value;
            }
            else
            {
                stateResolvers.TryAdd(name, () => value);
            }
        }

        private Func<object> FindResolverForState<T>(string name)
        {
            if (stateProviders == null)
            {
                stateProviders = HttpContext.RequestServices.GetServices<IStateProvider>();
            }

            var resolver = stateProviders.FirstOrDefault(m => m.Name == name).Get();
            if (resolver == null)
            {
                return () => default(T);
            }            
            return () => resolver(HttpContext);
        }
        #endregion
    }
}

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
using IdentityModel.Client;
using System.Net.Http;

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

        public HttpContext Context => httpContextAccessor.HttpContext;
        public bool Logined => User.LoginName != "guest";
        public bool IsRoot => User.LoginName == "root";
        public bool IsAdmin => User.LoginName == "admin";
        public string InRoles => $"({User.Rbac_UAs.Join(ua => ua.Role_Id)})";
        public IList<long> Roles => User.Rbac_UAs.Select(ua => ua.Role_Id).ToList();
        public IDictionary<string, object> Params => User.Rbac_Params.ToDictionary(p => p.Name, p => (object)p.Value)
            .Merge(new Dictionary<string, object>
            {
                { "userid" , user.Id },
                { "loginname", user.LoginName },
                { "username", user.UserName },
                { "refcode", user.RefCode },
                { "nickname", user.NickName },
                { "scaleid", user.Scale_Id },
                { "inroles", InRoles },
                { "roleids", Roles }
            });
        public IDictionary<string, string> Auths => User.Rbac_Auths.ToDictionary(a => a.AuthType, a => a.AuthId);

        public Rbac_User User
        {
            get
            {
                if (user == null)
                {
                    user = Get<Rbac_User>("CurrentUser");

                    // check login
                    if (user == null)
                    {
                        Context.Response.Redirect("/plat/account/logout");
                    }
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

        private Task Login(Rbac_User user, bool persistent)
        {
            this.user = user;

            logger.LogDebug($"Login with the user \"{user.Id}:{user.LoginName}\"");
            var ticket = Membership.AuthenticationTicket(user.HashId, user.LoginName, persistent);
            return Context.SignInAsync(ticket.AuthenticationScheme, ticket.Principal, ticket.Properties);
        }

        public async Task<Token> GetToken(string userName, string password)
        {
            logger.LogDebug($"Get token begin with UserName \"{userName}\"");

            var settings = App.AppConfig.Settings;
            var oauthServer = settings.GetValue<string>("oauth.server");
            var oauthClient = settings.GetValue<string>("oauth.client");
            var oauthSecret = settings.GetValue<string>("oauth.secret");
            var oauthScope = settings.GetValue<string>("oauth.scope");

            // https://identitymodel.readthedocs.io/en/latest/client/discovery.html
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(oauthServer);
            Check.Assert(disco.IsError, $"OAuth认证服务器不能访问");

            // https://identitymodel.readthedocs.io/en/latest/client/token.html#requesting-a-token-using-the-password-grant-type
            var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                // "/connect/token"
                Address = disco.TokenEndpoint,
                ClientId = oauthClient,
                ClientSecret = oauthSecret,
                Scope = oauthScope,
                UserName = userName,
                Password = password
            });
            Check.Assert(token.IsError, token.ErrorDescription);

            logger.LogDebug("Get token end with: " + oauthServer + ", SUCCESS");
            return new Token
            {
                Access_token = token.AccessToken,
                Refresh_token = token.RefreshToken,
                Token_type = token.TokenType,
                Expires_in = token.ExpiresIn,
                Created = DateTime.Now
            };
        }

        public void UpdateAuth(long appId, string authId, string authType, bool needSaveUser)
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

        public void Logout()
        {
            user = userService.QueryByUserName("guest");
        }

        public bool IsInRole(long roleId) => Roles.Contains(roleId);

        public bool CheckAccess(long refId)
        {
            throw new NotImplementedException();
        }

        public string Replace(string str, bool replaceQuery = true)
        {
            if (!str.HasValue())
            {
                return str;
            }
            if (replaceQuery)
            {
                str = App.ReplaceQuery(str);
            }
            return str.Replace(Params);
        }

        public long GetDefaultScaleId()
        {
            return Logined ? User.Scale_Id : Membership.Default_ScaleId;
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
                stateProviders = Context.RequestServices.GetServices<IStateProvider>();
            }

            var resolver = stateProviders.FirstOrDefault(m => m.Name == name).Get();
            if (resolver == null)
            {
                return () => default(T);
            }            
            return () => resolver(Context);
        }
        #endregion
    }
}

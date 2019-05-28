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
using Acesoft.Util;

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

        private IDictionary<string, object> @params;
        public IDictionary<string, object> Params
        {
            get
            {
                if (@params == null)
                {
                    @params = User.Rbac_Params.ToDictionary(p => p.Name, p => (object)p.Value);

                    @params.Add("userid", user.Id);
                    @params.Add("loginname", user.LoginName);
                    @params.Add("username", user.UserName);
                    @params.Add( "refcode", user.RefCode);
                    @params.Add( "nickname", user.NickName);
                    @params.Add( "scaleid", user.Scale_Id);
                    @params.Add( "inroles", InRoles);
                    @params.Add( "roleids", Roles);

                    if (Auths.ContainsKey("wechat"))
                    {
                        @params.Add("appid", auths["wechat"].App_Id);
                    }
                }
                return @params;
            }
        }

        private IDictionary<string, Rbac_Auth> auths;
        public IDictionary<string, Rbac_Auth> Auths
        {
            get
            {
                if (auths == null)
                {
                    auths = User.Rbac_Auths.ToDictionary(a => a.AuthType);
                }
                return auths;
            }
        }

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
            var user = userService.CheckUser(userName, password);
            return Login(user, persistent);
        }

        public Task Login(Rbac_User user, bool persistent)
        {
            this.user = user;

            var userName = user.LoginName ?? user.Mobile;
            var authId = user.Rbac_Auths.FirstOrDefault()?.AuthId ?? "none";
            var ticket = Membership.AuthenticationTicket(user.HashId, userName, authId, persistent);

            logger.LogDebug($"Login with the user \"{user.Id}:{userName}\"");
            return Context.SignInAsync(ticket.AuthenticationScheme, ticket.Principal, ticket.Properties);
        }

        public async Task<Token> GetToken(string userName, string password)
        {
            logger.LogDebug($"Get token begin with UserName \"{userName}\"");

            var settings = App.AppConfig.Settings;
            var oauthServer = settings.GetValue("oauth.server", App.GetWebRoot(true));
            var oauthClient = settings.GetValue<string>("oauth.client");
            var oauthSecret = settings.GetValue<string>("oauth.secret");
            var oauthScope = settings.GetValue<string>("oauth.scope");
            logger.LogDebug($"Request OAuth server \"{oauthServer}\" with Scope \"{oauthScope}\"");

            // https://identitymodel.readthedocs.io/en/latest/client/discovery.html
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = oauthServer,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            Check.Assert(disco.IsError, disco.Error);

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
            Check.Assert(token.IsError, token.ErrorDescription ?? token.Exception?.GetMessage());

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

        public async Task<Token> RefreshToken(string refreshToken)
        {
            logger.LogDebug($"Refresh token begin with \"{refreshToken}\"");

            var settings = App.AppConfig.Settings;
            var oauthServer = settings.GetValue("oauth.server", App.GetWebRoot(true));
            var oauthClient = settings.GetValue<string>("oauth.client");
            var oauthSecret = settings.GetValue<string>("oauth.secret");
            var oauthScope = settings.GetValue<string>("oauth.scope");
            logger.LogDebug($"Request OAuth server \"{oauthServer}\" with Scope \"{oauthScope}\"");

            // https://identitymodel.readthedocs.io/en/latest/client/discovery.html
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = oauthServer,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            Check.Assert(disco.IsError, disco.Error);

            // https://identitymodel.readthedocs.io/en/latest/client/token.html#requesting-a-token-using-the-password-grant-type
            var token = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                // "/connect/token"
                Address = disco.TokenEndpoint,
                ClientId = oauthClient,
                ClientSecret = oauthSecret,
                Scope = oauthScope,
                RefreshToken = refreshToken
            });
            Check.Assert(token.IsError, token.ErrorDescription ?? "RefreshToken已失效");

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

        public void Logout()
        {
            user = userService.QueryByUserName("guest", "none");
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

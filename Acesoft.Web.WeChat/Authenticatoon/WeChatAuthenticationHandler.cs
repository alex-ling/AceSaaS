using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Acesoft.Rbac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Senparc.Weixin.WxOpen.Containers;

namespace Acesoft.Web.WeChat.Authenticatoon
{
    public class WeChatAuthenticationHandler : AuthenticationHandler<WechatAuthenticationOptions>
    {
        private IUserService userService;

        public WeChatAuthenticationHandler(
            IOptionsMonitor<WechatAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            this.userService = userService;
        }    

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Headers["wechat_token"];
            if (token.HasValue() && SessionContainer.GetSession(token) != null)
            {
                // check right for later.
                var ticket = Membership.AuthenticationTicket(token, "", "", true, Scheme.Name);
                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return await Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}

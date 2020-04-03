using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Acesoft.Rbac;
using Acesoft.Util;
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
            string tokens = Request.Headers["Authorization"];
            if (tokens.HasValue() && tokens.Split(' ').First() == "Wechat")
            {
                var items = tokens.Split(' ').Last().Split('-');

                // weopen直接验证通过，weapp检查会话是否超时
                if (items[1] == CryptoHelper.ComputeMD5("WEOPEN", items[0]) 
                    || SessionContainer.GetSession(items[1]) != null)
                {
                    // check right for later.
                    var ticket = Membership.AuthenticationTicket(items[0], "", "", true, Scheme.Name);
                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            return await Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Rbac;
using Acesoft.Rbac.Entity;
using Acesoft.Util.Helper;
using Acesoft.Web.WeChat.Entity;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Weixin.BrowserUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;

namespace Acesoft.Web.WeChat
{
    public static class AccessControlExtensions
    {
        public static bool TryWechatAutoLogin(this IAccessControl ac, bool external, out string openId)
        {
            var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();

            try
            {
                var code = App.GetQuery("code", "");
                var result = OAuthApi.GetAccessToken(app.AppId, app.AppSecret, code);
                openId = result.openid;
            }
            catch
            {
                openId = "";
            }

            if (openId.HasValue())
            {
                if (external) return true;

                var service = ac.Context.RequestServices.GetService<IUserService>();
                var user = service.QueryByAuth(app.Id, openId);
                if (user != null)
                {
                    ac.Login(user, true).Wait();
                    return true;
                }
                return false;
            }

            return false;
        }

        public static async Task WechatLogin(this IAccessControl ac, string userName, string password, string openId)
        {
            var user = ac.CheckUser(userName, password);
            var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();
            var userService = ac.Context.RequestServices.GetService<IUserService>();

            var userInfoJson = UserApi.Info(app.AppId, openId);
            if (userInfoJson != null)
            {
                user.NickName = userInfoJson.nickname;
                user.Photo = userInfoJson.headimgurl;
                userService.Update(user);
            }

            userService.UpdateAuth(user, app.Id, openId, "wechat");
            await ac.Login(user, true);
        }

        public static bool WeChatAuthorize(this IAccessControl ac, string openIdParamName)
        {
            var context = ac.Context;
            if (!context.SideInWeixinBrowser()) return true;

            try
            {
                if (ac.Logined && ac.Auths.ContainsKey("wechat"))
                {
                    var returnUrl2 = App.GetQuery("ReturnUrl", "/wechat");
                    if (returnUrl2.StartsWith("http"))
                    {
                        returnUrl2 = UrlHelper.Append(returnUrl2, openIdParamName, ac.Auths["wechat"].AuthId);
                    }
                    context.Response.Redirect(returnUrl2);
                    return true;
                }

                if (ac.Logined) ac.Logout();

                var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();
                var returnUrl = App.GetQuery("ReturnUrl", "/wechat");
                var state = App.GetQuery("state", "state");
                var scope = (OAuthScope)App.GetQuery("scope", 0);

                var redirectUrl = $"{App.GetWebRoot(true)}plat/account/wechat?appid={app.Id}&redirect_uri={returnUrl}";
                var authorizeUrl = OAuthApi.GetAuthorizeUrl(app.AppId, redirectUrl, state, scope);
                context.Response.Redirect(authorizeUrl);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static JsApiToken GetJsApiToken(this IAccessControl ac, bool resetUrl)
        {
            var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();

            var timestamp = JSSDKHelper.GetTimestamp();
            var noncestr = JSSDKHelper.GetNoncestr();
            var jsApiTicket = JsApiTicketContainer.GetJsApiTicket(app.AppId);
            var url = ac.Context.Request.GetUrl();

            if (resetUrl) url = UrlHelper.Append(url, "t", timestamp);
            var signature = JSSDKHelper.GetSignature(jsApiTicket, noncestr, timestamp, url);
            return new JsApiToken
            {
                AppId = app.AppId,
                Ticket = jsApiTicket,
                Timestamp = timestamp,
                Nonce = noncestr,
                Signature = signature
            };
        }
    }
}

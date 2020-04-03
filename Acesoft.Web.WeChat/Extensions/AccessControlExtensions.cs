using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Rbac;
using Acesoft.Rbac.Entity;
using Acesoft.Util;
using Acesoft.Util.Helper;
using Acesoft.Web.WeChat.Entity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

using Senparc.Weixin.BrowserUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Helpers;

namespace Acesoft.Web.WeChat
{
    public static class AccessControlExtensions
    {
        #region wechat
        public static string GetWeopenToken(this IAccessControl ac)
        {
            return $"{ac.User.HashId}-{CryptoHelper.ComputeMD5("WEOPEN", ac.User.HashId)}";
        }

        public static bool TryWechatAutoLogin(this IAccessControl ac, bool external, bool autoCreate, out string openId)
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
                else if (autoCreate)
                {
                    var wechatService = ac.Context.RequestServices.GetService<IWechatService>();
                    var userInfo = UserApi.Info(app.AppId, openId);
                    user = wechatService.WeopenRegist(app, userInfo, null);
                    ac.Login(user, true).Wait();
                    return true;
                }
                return false;
            }

            return false;
        }

        public static async Task WechatLogin(this IAccessControl ac, string userName, string password, string openId)
        {
            var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();
            var userService = ac.Context.RequestServices.GetService<IUserService>();
            var user = userService.CheckUser(userName, password);

            var userInfoJson = UserApi.Info(app.AppId, openId);
            if (userInfoJson != null)
            {
                user.NickName = userInfoJson.nickname;
                user.Photo = userInfoJson.headimgurl;
                user.Province = userInfoJson.province;
                user.City = userInfoJson.city;
                user.Country = userInfoJson.country;
                user.UnionId = userInfoJson.unionid;
                userService.Update(user);
            }

            userService.UpdateAuth(user, app.Id, openId, "wechat");
            await ac.Login(user, true);
        }

        public static bool WeChatAuthorize(this IAccessControl ac, string openIdParamName)
        {
            var context = ac.Context;

            // 是否微信运行
            if (!context.SideInWeixinBrowser())
            {
                return true;
            }

            // 检查是否wechat oauth, 是否传入appid
            if (!App.GetQuery("appid", "").HasValue())
            {
                return true;
            }

            try
            {
                var returnUrl = App.GetQuery("returnurl", "/wechat");
                if (ac.Logined && ac.Auths.ContainsKey("wechat"))
                {
                    if (returnUrl.StartsWith("http"))
                    {
                        returnUrl = UrlHelper.Append(returnUrl, openIdParamName, ac.Auths["wechat"].AuthId);
                    }
                    context.Response.Redirect(returnUrl);
                    return true;
                }
                else if (ac.Logined)
                {
                    // logout first
                    ac.Logout();
                }

                var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();
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
        #endregion

        #region weapp
        public static string TryWeappAutoLogin(this IAccessControl ac, JObject data, out string errMsg)
        {
            errMsg = null;

            var app = ac.Context.RequestServices.GetService<IWeChatContainer>().GetApp();
            var session = data.GetValue("sessionid", key => SessionContainer.GetSession(key));
            if (session == null)
            {
                // session未建立，通过code登录换取session
                var result = WeChatApi.WxLogin(app, data.GetValue("code", ""));
                if (result.ErrorCodeValue == 0)
                {
                    // success，成功时更新session
                    session = SessionContainer.UpdateSession(null, result.openid, result.session_key, result.unionid);
                }
                else
                {
                    errMsg = result.errmsg;
                }
            }

            if (session != null)
            {
                // 根据OpenID自动登录
                var service = ac.Context.RequestServices.GetService<IUserService>();
                var user = service.GetByAuth(app.Id, session.OpenId);
                
                if (user == null)
                {
                    // OpenID对应的用户不存在，解密客户端数据
                    var cryptedData = data.GetValue("encryptedData", "");
                    var iv = data.GetValue("iv", "");
                    var userInfo = EncryptHelper.DecodeUserInfoBySessionId(session.Key, cryptedData, iv);

                    // 自动创建用户
                    var mobile = data.GetValue<string>("mobile");
                    var wechatService = ac.Context.RequestServices.GetService<IWechatService>();
                    user = wechatService.WeappRegist(app, userInfo, mobile);
                }

                return $"{user.HashId}-{session.SessionKey}";
            }

            return null;
        }
        #endregion
    }
}

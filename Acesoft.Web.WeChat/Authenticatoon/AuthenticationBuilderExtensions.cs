using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.WeChat.Authenticatoon
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder)
        {
            return builder.AddWechat("Wechat", options => { });
        }

        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder, Action<WechatAuthenticationOptions> configureOptions)
        {
            return builder.AddWechat("Wechat", configureOptions);
        }

        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder, string authenticationScheme, Action<WechatAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<WechatAuthenticationOptions, WeChatAuthenticationHandler>(authenticationScheme, configureOptions);
        }
    }
}

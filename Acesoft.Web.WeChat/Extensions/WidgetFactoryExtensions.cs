using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.UI;
using Senparc.Weixin.BrowserUtility;

namespace Acesoft.Web.WeChat
{
    public static class WidgetFactoryExtensions
    {
        public static void InitWeChatScript(this WidgetFactory ace, Action<IList<string>> actionScript, bool resetUrl = false, bool debug = false)
        {
            if (App.Context.SideInWeixinBrowser())
            {
                var jsApiToken = ace.AC.GetJsApiToken(resetUrl);
                var sb = new StringBuilder();
                var list = new List<string>();
                actionScript(list);

                sb.Append("$(function() { AX.wxInit({ ");
                sb.Append($"appId:'{jsApiToken.AppId}',");
                sb.Append($"timestamp:'{jsApiToken.Timestamp}',");
                sb.Append($"nonce:'{jsApiToken.Nonce}',");
                sb.Append($"signature:'{jsApiToken.Signature}',");
                sb.Append($"jsApi:['{list.Join("','")}'],");
                sb.Append($"resetUrl:{resetUrl.ToString().ToLower()},");
                sb.Append($"debug:{debug.ToString().ToLower()}");
                sb.Append(" }) })");

                ace.Context.AppendInitScripts(sb.ToString());
            }
        }
    }
}

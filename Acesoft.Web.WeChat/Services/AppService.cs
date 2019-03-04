using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Containers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using MP = Senparc.Weixin.MP.Entities.Request;
using WXOpen = Senparc.Weixin.WxOpen.Entities.Request;

namespace Acesoft.Web.WeChat.Services
{
	public class AppService : Service<Wx_App>, IAppService
	{
		public new Wx_App Get(long id)
		{
			return Session.QueryFirst<Wx_App>(
                new RequestContext("wx", "get_wx_app")
                .SetParam(new
			    {
				    id
			    })
            );
		}

        public bool Check(Wx_App app, MP.PostModel model)
        {
            string token = model.Token ?? app.Token;
            if (CheckSignature.Check(model.Signature, model.Timestamp, model.Nonce, token))
            {
                model.Token = token;
                model.AppId = app.AppId;
                model.EncodingAESKey = app.AesKey;
                return true;
            }
            return false;
        }

        public bool Check(Wx_App app, WXOpen.PostModel model)
        {
            string token = model.Token ?? app.Token;
            if (CheckSignature.Check(model.Signature, model.Timestamp, model.Nonce, token))
            {
                model.Token = token;
                model.AppId = app.AppId;
                model.EncodingAESKey = app.AesKey;
                return true;
            }
            return false;
        }

        public object GetToken(Wx_App app, string secret)
        {
            Acesoft.Check.Require(secret == app.Secret, "传入的AppId与Secret不匹配！");

            var accessTokenResult = AccessTokenContainer.GetAccessTokenResult(app.AppId);
            var accessTokenBag = BaseContainer<AccessTokenBag>.TryGetItem(app.AppId);
            var jsApiTicketResult = JsApiTicketContainer.GetJsApiTicketResult(app.AppId);
            var jsApiTicketBag = BaseContainer<JsApiTicketBag>.TryGetItem(app.AppId);
            return new
            {
                appid = app.AppId,
                token = accessTokenResult.access_token,
                token_expired = accessTokenBag.AccessTokenExpireTime.ToUnixTimeSeconds(),
                ticket = jsApiTicketResult.ticket,
                ticket_expired = jsApiTicketBag.JsApiTicketExpireTime.ToUnixTimeSeconds()
            };
        }
    }
}

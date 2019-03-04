using System.IO;

using Acesoft.Util;
using Acesoft.Web.WeChat.Entity;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;

namespace Acesoft.Web.WeChat
{
	public static class WeChatApi
	{
		public static UploadImgResult UploadImg(string accessTokenOrAppId, string src)
		{
			if (src.StartsWith("https://mmbiz.qpic.cn/") || src.StartsWith("http://mmbiz.qpic.cn/"))
			{
				return null;
			}

			if (src.StartsWith("http"))
			{
				return ApiHandlerWapper.TryCommonApi(accessToken =>
				{
					string url = $"{"https"}://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={accessToken.AsUrlData()}";
					var fileBytes = HttpHelper.HttpDownload(src);
					return Post.GetResult<UploadImgResult>(HttpHelper.HttpUpload(url, fileBytes, Path.GetFileName(src), "media"));
				}, accessTokenOrAppId);
			}

			return MediaApi.UploadImg(accessTokenOrAppId, App.GetLocalResource(src));
		}

		public static UploadForeverMediaResult UploadForeverMedia(string accessTokenOrAppId, string src)
		{
			if (src.StartsWith("http"))
			{
				return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
				{
					string url = $"{"https"}://api.weixin.qq.com/cgi-bin/material/add_material?access_token={accessToken.AsUrlData()}";
					var fileBytes = HttpHelper.HttpDownload(src);
					return Post.GetResult<UploadForeverMediaResult>(HttpHelper.HttpUpload(url, fileBytes, Path.GetFileName(src), "media"));
				}, accessTokenOrAppId);
			}

			return MediaApi.UploadForeverMedia(accessTokenOrAppId, App.GetLocalResource(src));
		}

        public static JsCode2JsonResult WxLogin(Wx_App app, string code)
        {
            return SnsApi.JsCode2Json(app.AppId, app.AppSecret, code);
        }
    }
}

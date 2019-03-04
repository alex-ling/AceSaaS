using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;

namespace Acesoft.Web.WeChat
{
	public interface INewsService : IService<Wx_News>
	{
		IEnumerable<Wx_News> GetNews(string ids);

        void UpdateThumbMedia(Wx_News news);

        void UploadNews(long appId, string newsIds, int count, string mediaId, string wxUrl);

        WxJsonResult UploadNews(Wx_App app, string newsIds, int openComment = 1);
    }
}

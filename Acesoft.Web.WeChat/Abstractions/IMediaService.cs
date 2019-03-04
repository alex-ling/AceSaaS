using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace Acesoft.Web.WeChat
{
	public interface IMediaService : IService<Wx_Media>
	{
		IEnumerable<Wx_Media> GetLocalMedias(long appId, string ids);

		IEnumerable<Wx_Media> GetRemoteMedias(long appId, string ids);

        void UpdateMedia(long id, string mediaId, string wxUrl);

        UploadForeverMediaResult UploadMedia(Wx_App app, string mediaUrl);

        void UploadMedias(Wx_App app, string mediaIds);

        void DeleteMedias(Wx_App app, string mediaIds);

        void PreviewMedia(Wx_App app, long mediaId, string wxNames);

        WxJsonResult SendMedia(Wx_App app, long mediaId, bool isToAll = true);
    }
}

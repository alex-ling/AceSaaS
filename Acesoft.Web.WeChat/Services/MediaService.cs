using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace Acesoft.Web.WeChat.Services
{
	public class MediaService : Service<Wx_Media>, IMediaService
	{
		public IEnumerable<Wx_Media> GetLocalMedias(long appId, string ids)
		{
			return Session.Query<Wx_Media>(
                new RequestContext("wx", "get_wx_media_local")
                .SetParam(new
			    {
				    appId,
				    ids = ids.Split<long>(',')
			    })
            );
		}

		public IEnumerable<Wx_Media> GetRemoteMedias(long appId, string ids)
		{
			return Session.Query<Wx_Media>(
                new RequestContext("wx", "get_wx_media_remote")
                .SetParam(new
			    {
				    appId,
				    ids = ids.Split<long>(',')
			    })
            );
		}

        public void UpdateMedia(long id, string mediaId, string wxUrl)
        {
            Session.Execute(new RequestContext("wx", "exec_wx_media_update")
                .SetParam(new
                {
                    id,
                    mediaId,
                    wxUrl
                })
            );
        }

        public UploadForeverMediaResult UploadMedia(Wx_App app, string mediaUrl)
        {
            if (!mediaUrl.HasValue())
            {
                throw new AceException("上传的素材不存在！");
            }
            return WeChatApi.UploadForeverMedia(app.AppId, mediaUrl);
        }

        public void UploadMedias(Wx_App app, string mediaIds)
        {
            foreach (Wx_Media localMedia in GetLocalMedias(app.Id, mediaIds))
            {
                UploadForeverMediaResult uploadForeverMediaResult = UploadMedia(app, localMedia.Url);
                UpdateMedia(localMedia.Id, uploadForeverMediaResult.media_id, uploadForeverMediaResult.url);
            }
        }

        public void DeleteMedias(Wx_App app, string mediaIds)
        {
            foreach (var remoteMedia in GetRemoteMedias(app.Id, mediaIds))
            {
                MediaApi.DeleteForeverMedia(app.AppId, remoteMedia.MediaId);
            }
        }

        public void PreviewMedia(Wx_App app, long mediaId, string wxNames)
        {
            var media = Get(mediaId);
            wxNames.Split(',').Each(wxName =>
            {
                GroupMessageApi.SendGroupMessagePreview(app.AppId, GetMessageType(media.Type), media.MediaId, null, wxName);
            });
        }

        public WxJsonResult SendMedia(Wx_App app, long mediaId, bool isToAll = true)
        {
            var media = Get(mediaId);
            var result = GroupMessageApi.SendGroupMessageByGroupId(app.AppId, null, media.MediaId, GetMessageType(media.Type), isToAll);
            WriteSendLog(mediaId, result);
            return result;
        }

        private Wx_Send WriteSendLog(long mediaId, SendResult result)
        {
            Wx_Send wX_Send = new Wx_Send();
            wX_Send.InitializeId();
            wX_Send.DCreate = DateTime.Now;
            wX_Send.Media_Id = mediaId;
            wX_Send.MsgId = result.msg_id;
            wX_Send.ErrCode = $"{result.ErrorCodeValue}:{result.errcode}";
            wX_Send.ErrMsg = result.errmsg;
            Session.Insert(wX_Send);

            return wX_Send;
        }

        private GroupMessageType GetMessageType(WxMedia mediaType)
        {
            switch (mediaType)
            {
                case WxMedia.news:
                    return GroupMessageType.mpnews;
                case WxMedia.image:
                    return GroupMessageType.image;
                case WxMedia.text:
                    return GroupMessageType.text;
                case WxMedia.video:
                    return GroupMessageType.video;
                case WxMedia.voice:
                    return GroupMessageType.voice;
                default:
                    return GroupMessageType.wxcard;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Platform;
using Acesoft.Web.WeChat.Entity;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;

namespace Acesoft.Web.WeChat.Services
{
    public class ActivityService : Service<Wx_Activity>, IActivityService
    {
        public void CreateQrCode(Wx_App app, string activityIds)
        {
            activityIds.Split<long>().Each(activityId =>
            {
                var activity = Get(activityId);
                if (activity != null)
                {
                    var result = QrCodeApi.Create(app.AppId, 2592000, 1, QrCode_ActionName.QR_LIMIT_STR_SCENE, $"{activityId}");
                    activity.QrCode = result.url;
                    activity.DUpdate = DateTime.Now;
                    UpdateQrCode(activity);
                }
            });
        }

        public void SendActivity(Wx_App app, Wx_Activity activity, string openId)
        {
            var articles = new List<Article>();
            articles.Add(new Article
            {
                Title = activity.Name,
                Description = activity.Remark,
                PicUrl = activity.Poster,
                Url = activity.Url
            });

            CustomApi.SendNews(app.AppId, openId, articles);
        }

        public void UpdateQrCode(Wx_Activity activity)
        {
            var sql = "update wx_activity set dupdate=@dupdate,qrcode=@qrcode where id=@id";
            Session.Execute(sql, new
            {
                activity.Id,
                activity.DUpdate,
                activity.QrCode
            });
        }
    }
}

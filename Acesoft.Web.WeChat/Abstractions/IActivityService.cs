using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;

namespace Acesoft.Web.WeChat
{
	public interface IActivityService : IService<Wx_Activity>
	{
        void CreateQrCode(Wx_App app, string activityIds);
        void UpdateQrCode(Wx_Activity activity);
        void SendActivity(Wx_App app, Wx_Activity activity, string openId);
    }
}

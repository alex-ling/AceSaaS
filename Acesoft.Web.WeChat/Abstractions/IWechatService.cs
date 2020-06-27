using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Rbac.Entity;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.WxOpen.Entities;

namespace Acesoft.Web.WeChat
{
    public interface IWechatService
    {
        Rbac_User WechatLogin(long appId, string authId);
        Rbac_User WeappRegist(Wx_App app, DecodedUserInfo userInfo, string mobile);
        Rbac_User WeopenRegist(Wx_App app, UserInfoJson userInfo, string mobile);
    }
}

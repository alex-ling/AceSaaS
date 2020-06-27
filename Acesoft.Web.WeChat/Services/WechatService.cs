using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.WxOpen.Entities;

namespace Acesoft.Web.WeChat.Services
{
    public class WechatService : Service<Rbac_User>, IWechatService
    {
        public Rbac_User WechatLogin(long appId, string authId)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("wx", "exec_wx_login")
                .SetParam(new
                {
                    appId,
                    authId
                })
            );
        }

        public Rbac_User WeappRegist(Wx_App app, DecodedUserInfo userInfo, string mobile = null)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("wx", "exec_wx_regist")
                .SetParam(new
                {
                    newid = App.IdWorker.NextId(),
                    mobile = mobile,
                    nickname = userInfo.nickName,
                    photo = userInfo.avatarUrl,
                    province = userInfo.province,
                    city = userInfo.city,
                    country = userInfo.country,
                    appid = app.Id,
                    authtype = "wechat",
                    authid = userInfo.openId,
                    unionid = userInfo.unionId
                })
            );
        }

        public Rbac_User WeopenRegist(Wx_App app, UserInfoJson userInfo, string mobile = null)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("wx", "exec_wx_regist")
                .SetParam(new
                {
                    newid = App.IdWorker.NextId(),
                    mobile = mobile,
                    nickname = userInfo.nickname,
                    photo = userInfo.headimgurl,
                    province = userInfo.province,
                    city = userInfo.city,
                    country = userInfo.country,
                    appid = app.Id,
                    authtype = "wechat",
                    authid = userInfo.openid,
                    unionid = userInfo.unionid
                })
            );
        }
    }
}

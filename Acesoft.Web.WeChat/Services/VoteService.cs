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
    public class VoteService : Service<Wx_VoteLog>, IVoteService
    {
        public WxVoteResult GetVote(long voteId, string openId)
        {
            return Session.QueryMultiple(
                new RequestContext("wx.get_wx_vote")
                .SetParam(new { voteId, openId }), reader =>
                {
                    return new WxVoteResult
                    {
                        Vote = reader.Read<Wx_Vote>(true).SingleOrDefault(),
                        Result = reader.Read<WxUserResult>(true).SingleOrDefault(),
                        VoteUses = reader.Read(true).Select(a => (long)a.id).ToList()
                    };
                }
            );
        }

        public int Vote(long voteItemId, string openId, string content = null)
        {
            return Session.ExecuteScalar<int>(
                new RequestContext("wx.exec_wx_vote")
                .SetParam(new
                {
                    newId = App.IdWorker.NextId(),
                    voteItemId,
                    openId,
                    content
                })
            );
        }
    }
}

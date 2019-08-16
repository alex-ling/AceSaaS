using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;

namespace Acesoft.Web.WeChat
{
	public interface IVoteService : IService<Wx_VoteLog>
	{
        WxVoteResult GetVote(long voteId, string openId);
        int Vote(long voteItemId, string openId, string content = null);
    }
}

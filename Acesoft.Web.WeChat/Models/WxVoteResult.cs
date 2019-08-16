using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.WeChat.Entity;

namespace Acesoft.Web.WeChat
{
    public class WxVoteResult
    {
        public Wx_Vote Vote { get; set; }
        public WxUserResult Result { get; set; }
        public List<long> VoteUses { get; set; }
    }

    public class WxUserResult
    {
        public string OpenId { get; set; }
        public int VoteCount { get; set; }
        public int DayVoteCount { get; set; }
        public int VoteLeaf { get; set; }
    }
}

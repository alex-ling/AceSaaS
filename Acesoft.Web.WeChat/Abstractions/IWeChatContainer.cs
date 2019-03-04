using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.WeChat.Entity;

namespace Acesoft.Web.WeChat
{
    public interface IWeChatContainer
    {
        Wx_App GetApp();
    }
}

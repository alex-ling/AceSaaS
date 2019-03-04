using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Entities;

namespace Acesoft.Web.WeChat
{
	public interface IMenuService : IService<Wx_Menu>
	{
		IEnumerable<Wx_Menu> GetMenus(long appId);

        GetMenuResult GetMenu(Wx_App app);

        WxJsonResult CreateMenu(Wx_App app);

        WxJsonResult DeleteMenu(Wx_App app);
    }    
}

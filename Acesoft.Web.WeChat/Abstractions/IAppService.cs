using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using MP = Senparc.Weixin.MP.Entities.Request;
using WXOpen = Senparc.Weixin.WxOpen.Entities.Request;

namespace Acesoft.Web.WeChat
{
	public interface IAppService : IService<Wx_App>
	{
		new Wx_App Get(long id);

        bool Check(Wx_App app, MP.PostModel model);

        bool Check(Wx_App app, WXOpen.PostModel model);

        object GetToken(Wx_App app, string secret);
    }
}

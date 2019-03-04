using Senparc.NeuChar.Context;
using Senparc.NeuChar.Entities;

namespace Acesoft.Web.WeChat.WeApp
{
	public class WeAppContext : MessageContext<IRequestMessageBase, IResponseMessageBase>
	{
		public WeAppContext()
		{
			base.MessageContextRemoved += WeAppContext_MessageContextRemoved;
		}

		private void WeAppContext_MessageContextRemoved(object sender, WeixinContextRemovedEventArgs<IRequestMessageBase, IResponseMessageBase> e)
		{
			WeAppContext weAppContext = e.MessageContext as WeAppContext;
		}
	}
}

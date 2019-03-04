using System.IO;

using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace Acesoft.Web.WeChat.WeOpen
{
	public class WeChatHandler : MessageHandler<WeChatContext>
	{
		public WeChatHandler(Stream inputStream, PostModel postModel, int maxRecordCount)
			: base(inputStream, postModel, maxRecordCount)
		{
		}

		public override void OnExecuting()
		{
			base.OnExecuting();
		}

		public override void OnExecuted()
		{
			base.OnExecuted();
		}

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            return new SuccessResponseMessage();
        }
    }
}

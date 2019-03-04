using System;
using System.IO;
using System.Xml.Linq;

using Senparc.NeuChar.Entities;
using Senparc.Weixin.WxOpen.Entities.Request;
using Senparc.Weixin.WxOpen.MessageHandlers;

namespace Acesoft.Web.WeChat.WeApp
{
    public class WeAppHandler : WxOpenMessageHandler<WeAppContext>
    {
        public override XDocument ResponseDocument { get; }

        public override XDocument FinalResponseDocument { get; }

        public WeAppHandler(Stream inputStream, PostModel postModel, int maxRecordCount)
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

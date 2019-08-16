using SuperSocket.SocketBase;
using System;

namespace Acesoft.IotNet.Iot
{
	public class IotSession : AppSession<IotSession, IotRequest>
	{
		public string SessionId { get; set; }

		public IotDevice Device { get; set; }

		protected override void OnInit()
		{
			base.OnInit();
		}

		protected override void OnSessionStarted()
		{
		}

		protected override void HandleUnknownRequest(IotRequest requestInfo)
		{
		}

		protected override void HandleException(Exception e)
		{
		}

		protected override void OnSessionClosed(CloseReason reason)
		{
			base.OnSessionClosed(reason);
		}
	}
}

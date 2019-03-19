using System;

using SuperSocket.SocketBase;

namespace Acesoft.IotNet.Api
{
	public class ApiSession : AppSession<ApiSession, ApiRequest>
	{
		protected override void OnSessionStarted()
		{
		}

		protected override void HandleUnknownRequest(ApiRequest requestInfo)
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

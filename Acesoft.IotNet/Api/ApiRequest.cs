using SuperSocket.SocketBase.Protocol;

namespace Acesoft.IotNet.Api
{
	public class ApiRequest : IRequestInfo
	{
		public string Key { get; set; }

		public string Tenant { get; set; }

		public string Cmd { get; set; }

		public string Body { get; set; }

		public ApiRequest(string data)
		{
			var items = data.Split('#');
            Tenant = items[1];
            Key = items[2];
			Cmd = items[3];
			Body = items[4];
		}
	}
}

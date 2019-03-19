using SuperSocket.SocketBase.Protocol;

namespace Acesoft.IotNet.Api
{
	public class ApiRequest : IRequestInfo
	{
		public string Key
		{
			get;
			set;
		}

		public string Action
		{
			get;
			set;
		}

		public string Cmd
		{
			get;
			set;
		}

		public string Body
		{
			get;
			set;
		}

		public ApiRequest(string source)
		{
			string[] array = source.Split('#');
			Key = array[1];
			Action = array[2];
			Cmd = array[3];
			Body = array[4];
		}
	}
}

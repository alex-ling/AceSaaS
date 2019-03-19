using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Acesoft.Util;

namespace Acesoft.IotNet.Api
{
	public class ApiServer : AppServer<ApiSession, ApiRequest>, IDispatcher
	{
		private IDispatcher iot;
		private ApiSession session;
		private readonly object syncObj = new object();

		public ApiServer() : base(new DefaultReceiveFilterFactory<ApiReceiverFilter, ApiRequest>())
		{
			NewSessionConnected += ApiServer_NewSessionConnected;
			SessionClosed += ApiServer_SessionClosed;
			NewRequestReceived += ApiServer_NewRequestReceived;
		}

		private void ApiServer_NewRequestReceived(ApiSession session, ApiRequest req)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"APIRece: {req.Action}-{req.Key}-{req.Cmd}: {req.Body}");
			Console.ResetColor();

			string key = req.Key;
			string action = req.Action;
			if (!(action == "cmd"))
			{
				if (action == "sync")
				{
					IotCmd(key, req.Cmd, DatetimeHelper.GetNowHex());
				}
				else
				{
					Send(session, key, "BACK", req.Cmd, "0不支持该操作！");
				}
			}
			else
			{
				IotCmd(key, req.Cmd, req.Body);
			}
		}

		private void Send(ApiSession session, string mac, string action, string cmd, string body)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"APISend: {action}-{mac}-{cmd}: {body}");
			Console.ResetColor();

			var bytes = EncodingHelper.ToBytes($"#{mac}#{action}#{cmd}#{body}#");
			session.Send(bytes, 0, bytes.Length);
		}

		public bool Dispatch(string mac, string action, string cmd, string data)
		{
			if (session == null || !session.Connected)
			{
				lock (syncObj)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("The ApiClient hasn't connected!");
					if (session == null || !session.Connected)
					{
						Console.WriteLine("Now connecting to the ApiClient 1st...");
						if (!ConnectToApiClient())
						{
							Thread.Sleep(100);
							Console.WriteLine("Now connecting to the ApiClient 2st...");
							ConnectToApiClient();
						}
					}

                    // waiting 100ms
                    Thread.Sleep(100);

                    if (session == null || !session.Connected)
					{
						Console.WriteLine("Connecting to the ApiClient failure!");
						Console.WriteLine($"Sending to ApiClient failed: {mac}-{action}");
						Console.ResetColor();
						base.Logger.Error($"Sending to ApiClient failed: {mac}-{action}-{cmd}-{data}");
						return false;
					}

					Console.WriteLine("Connecting to the ApiClient success!");
					Console.ResetColor();
				}
			}

			Send(session, mac, action, cmd, data);
			return true;
		}

		private bool ConnectToApiClient()
		{
			try
			{
                string value = ConfigHelper.GetAppSetting<string>("ApiClientUrl");
				using (var httpClient = new HttpClient())
				{
					return httpClient.GetAsync(value).Result.IsSuccessStatusCode;
				}
			}
			catch
			{
				return false;
			}
		}

		private void IotCmd(string mac, string cmd, string data)
		{
			iot.Dispatch(mac, "cmd", cmd, data);
		}

		private void ApiServer_SessionClosed(ApiSession session, CloseReason value)
		{
			Console.WriteLine($"API {session.RemoteEndPoint} has disconnected!");
		}

		private void ApiServer_NewSessionConnected(ApiSession session)
		{
			this.session = session;
			Console.WriteLine($"API {session.RemoteEndPoint} has connected!");
		}

		protected override void OnStarted()
		{
			iot = (Bootstrap.GetServerByName("IotServer") as IDispatcher);
			Console.WriteLine($"API Socket [{Config.Ip}:{Config.Port}] has started!");
		}

		protected override void OnStopped()
		{
			Console.WriteLine($"API Socket [{Config.Ip}:{Config.Port}] has stopped!");
		}
	}
}

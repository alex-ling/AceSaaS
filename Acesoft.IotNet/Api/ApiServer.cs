using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;

using Serilog;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Acesoft.Util;

namespace Acesoft.IotNet.Api
{
	public class ApiServer : AppServer<ApiSession, ApiRequest>, IDispatcher
	{
		private readonly object syncObj = new object();
        private readonly ILogger logger;
		private IDispatcher iot;
		private ApiSession session;

		public ApiServer() : base(new DefaultReceiveFilterFactory<ApiReceiverFilter, ApiRequest>())
		{
			NewSessionConnected += ApiServer_NewSessionConnected;
			SessionClosed += ApiServer_SessionClosed;
			NewRequestReceived += ApiServer_NewRequestReceived;
            logger = Log.ForContext<ApiServer>();
		}

		private void ApiServer_NewRequestReceived(ApiSession session, ApiRequest req)
		{
			logger.Debug($"API-Rece: {req.Action}-{req.Key}-{req.Cmd} {req.Body}");

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
			logger.Debug($"API-Send: {action}-{mac}-{cmd} {body}");

			var bytes = EncodingHelper.ToBytes($"#{mac}#{action}#{cmd}#{body}#");
			session.Send(bytes, 0, bytes.Length);
		}

		public bool Dispatch(string mac, string action, string cmd, string data)
		{
			if (session == null || !session.Connected)
			{
				lock (syncObj)
				{
					if (session == null || !session.Connected)
					{
                        ConnectToApiClient();

                        // waiting 100ms
                        Thread.Sleep(100);
                    }

                    if (session == null || !session.Connected)
					{
						return false;
					}
				}
			}

            Logger.Debug($"API-Send: {action}-{mac}-{cmd} {data}");
            Send(session, mac, action, cmd, data);
			return true;
		}

		private bool ConnectToApiClient()
		{
			try
            {
                string url = ConfigHelper.GetAppSetting<string>("ApiClientUrl");
                logger.Debug($"API-Conn: Connecting {{{url}}}");

				using (var httpClient = new HttpClient())
				{
					return httpClient.GetAsync(url).Result.IsSuccessStatusCode;
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
			logger.Debug($"API-Session-END: {session.RemoteEndPoint}");
		}

		private void ApiServer_NewSessionConnected(ApiSession session)
		{
			this.session = session;
			logger.Debug($"API-Session-BEN: {session.RemoteEndPoint}");
		}

		protected override void OnStarted()
		{
			iot = Bootstrap.GetServerByName("IotServer") as IDispatcher;

			logger.Debug($"API-Socket-START: {Config.Ip}:{Config.Port}");
		}

		protected override void OnStopped()
		{
			logger.Debug($"API-Socket-STOP: {Config.Ip}:{Config.Port}");
		}
	}
}

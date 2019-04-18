using System;
using System.Configuration;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;

using Serilog;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Acesoft.Util;
using Acesoft.IotNet.Iot;

namespace Acesoft.IotNet.Api
{
	public class ApiServer : AppServer<ApiSession, ApiRequest>, IDispatcher
	{
		private readonly object syncObj = new object();
        private readonly ILogger logger;
        private readonly ConcurrentDictionary<string, IotServer> servers = new ConcurrentDictionary<string, IotServer>();
        private readonly ConcurrentDictionary<string, ApiSession> sessions = new ConcurrentDictionary<string, ApiSession>();

		public ApiServer() : base(new DefaultReceiveFilterFactory<ApiReceiverFilter, ApiRequest>())
		{
			NewSessionConnected += ApiServer_NewSessionConnected;
			SessionClosed += ApiServer_SessionClosed;
			NewRequestReceived += ApiServer_NewRequestReceived;
            logger = Log.ForContext<ApiServer>();
		}

		private void ApiServer_NewRequestReceived(ApiSession session, ApiRequest req)
		{
			logger.Debug($"API-Rece: {req.Tenant}-{req.Key}-{req.Cmd} {req.Body}");

            // add session to cache.
            sessions.AddOrUpdate(req.Tenant, session, (key, s) => session);

            if (req.Cmd == "CONN")
            {
                Send(session, req.Tenant, req.Key, "BACK-CONN", "00ok");
            }
            else
            {
                var server = servers.GetOrAdd(req.Tenant, key =>
                {
                    logger.Debug($"API-IoTServer-START: {key}");
                    return Bootstrap.GetServerByName(key) as IotServer;
                });
                server.Dispatch(req.Tenant, req.Key, req.Cmd, req.Body);
            }
        }

		private void Send(ApiSession session, string tenant, string mac, string cmd, string body)
		{
			logger.Debug($"API-Send: {tenant}-{mac}-{cmd} {body}");

			var bytes = EncodingHelper.ToBytes($"#{tenant}#{mac}#{cmd}#{body}#");
			session.Send(bytes, 0, bytes.Length);
		}

		public bool Dispatch(string server, string mac, string cmd, string data)
		{
            sessions.TryGetValue(server, out ApiSession session);
            if (session == null || !session.Connected)
			{
				lock (syncObj)
				{
					if (session == null || !session.Connected)
					{
                        var s = servers.GetOrAdd(server, key =>
                        {
                            return Bootstrap.GetServerByName(key) as IotServer;
                        });
                        s.ConnectToApiClient();

                        // waiting 100ms
                        Thread.Sleep(100);
                    }

                    if (session == null || !session.Connected)
					{
						return false;
					}
				}
			}

            Logger.Debug($"API-Send: {server}-{mac}-{cmd} {data}");
            Send(session, server, mac, cmd, data);
			return true;
		}

		private void ApiServer_SessionClosed(ApiSession session, CloseReason value)
		{
			logger.Debug($"API-Session-END: {session.RemoteEndPoint}");
		}

		private void ApiServer_NewSessionConnected(ApiSession session)
		{
			logger.Debug($"API-Session-BGN: {session.RemoteEndPoint}");
		}

		protected override void OnStarted()
		{
			logger.Debug($"API-Socket-START: {Config.Ip}:{Config.Port}");
		}

		protected override void OnStopped()
		{
			logger.Debug($"API-Socket-STOP: {Config.Ip}:{Config.Port}");
		}
	}
}

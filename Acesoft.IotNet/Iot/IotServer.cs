using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Serilog;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Acesoft.Util;
using Acesoft.Security;
using System.Net.Http;

namespace Acesoft.IotNet.Iot
{
	public class IotServer : AppServer<IotSession, IotRequest>, IDispatcher
	{
		private readonly ConcurrentDictionary<string, IotSession> sessions = new ConcurrentDictionary<string, IotSession>();
        private readonly ILogger logger;
        private int interval;
		private IDispatcher api;

        public IotServer() : base(new IotReceiveFilterFactory())
		{
			NewSessionConnected += IotServer_NewSessionConnected;
			NewRequestReceived += IotServer_NewRequestReceived;
			SessionClosed += IotServer_SessionClosed;
            logger = Log.ForContext<IotServer>();
        }

        private string GetWeaHex(string mac)
        {
            var iotData = App.Cache.Get<IotData>($"iot_data_{mac}");
            if (iotData != null && iotData.Weather != null)
            {
                var pm25 = int.Parse(iotData.Weather.aqi.pm25);
                var temp = double.Parse(iotData.Weather.condition.temp);
                var humi = int.Parse(iotData.Weather.condition.humidity);
                return $"{NaryHelper.ToYmHex(pm25, 2)}{NaryHelper.ToYmHex(temp, 4)}{NaryHelper.ToYmHex(humi, 2)}";
            }
            return $"EEEEEEEE";
        }

		private void IotServer_NewRequestReceived(IotSession session, IotRequest req)
		{
			logger.Debug($"IoT-Rece: {session.RemoteEndPoint} {req.Device.Mac}-{req.SessionId} {req.Command}");

			IotRequest request = null;
			//var hasErrorRequest = false;
            //var hasErrorResponse = false;
			if (!req.CheckCrc16())
			{
				if (!req.Command.IsResponse)
				{
					request = req.ErrorCrc();
				}
				else
				{
					//hasErrorRequest = true;
				}
			}
			else
			{
				switch (req.Command.Name)
				{
				    case "00F1":
                    {
					    if (!req.CheckSession())
					    {
						    request = CreateErrorSession(req);
						    break;
					    }
					    if (!req.CheckValid())
					    {
						    request = req.ErrorDevice();
						    break;
					    }
					    request = req.Ok(interval.ToHex(2));
					    request.SessionId = RandomHelper.GetRandomHex(4);
					    session.SessionId = request.SessionId;
					    session.Device = req.Device;
					    sessions[req.Device.Mac] = session;
					    Task.Run(() => DoLogin(req));
					    break;
                    }
				    case "0001":
                    {
                        if (!req.CheckSession(sessions, session))
                        {
                            request = CreateErrorSession(req);
                            break;
                        }
                        Task.Run(() => DoUpload(req));
                        request = req.Ok(GetWeaHex(req.Device.Mac));
                        break;
                    }
				    case "0003":
                    {
                        if (!req.CheckSession(sessions, session))
                        {
                            request = CreateErrorSession(req);
                            break;
                        }
                        Task.Run(() => DoOnline(req));
                        request = req.Ok(DatetimeHelper.GetNowHex());
                        break;
                    }
				    default:
                    {
                        if (req.Command.IsResponse)
                        {
                            if (!req.CheckSession(sessions, session))
                            {
                                //hasErrorResponse = true;
                            }
                            else
                            {
                                api.Dispatch(Name, req.Device.Mac, $"BACK-{req.Command.Name}", $"{req.Command.DataHex}");
                            }
                        }
                        else if (!req.CheckSession(sessions, session))
                        {
                            request = CreateErrorSession(req);
                        }
                        else
                        {
                            Task.Run(() => DoUpData(req));
                            request = req.Ok();
                        }
                        break;
                    }
				}
			}

			if (request != null)
			{
				Send(session, request);
			}
		}

		private IotRequest CreateErrorSession(IotRequest req)
		{
			if (sessions.ContainsKey(req.Device.Mac))
			{
				Task.Run(delegate
				{
					DoLogout(req.Device);
				});
			}
			return req.ErrorSession();
        }

        public bool ConnectToApiClient()
        {
            try
            {
                var url = this.Config.Options["clientStartUrl"];
                logger.Debug($"IoT-Conn: Connecting {{{url}}}");

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

        private void Send(IotSession session, IotRequest req)
		{
            logger.Debug($"IoT-Send: {session.RemoteEndPoint} {req.Device.Mac}-{req.SessionId} {req.Command}");

            var bytes = req.BuildBytes();
			session.Send(bytes, 0, bytes.Length);
		}

		public bool Dispatch(string server, string mac, string cmd, string data)
		{
			if (sessions.TryGetValue(mac, out IotSession session) && session.Connected)
			{
				var request = IotRequest.CreateRequest((IotReceiveFilter)session.Items["filter"], mac, cmd, data);
				request.SessionId = session.SessionId;
				Send(session, request);
				return true;
			}

			api.Dispatch(server, mac, $"BACK-{cmd}", "FF设备已离线！");
			return false;
		}

		private void DoLogin(IotRequest req)
		{
			api.Dispatch(Name, req.Device.Mac, $"LOGIN-{req.Command.Name}", req.Command.DataHex);
		}

		private void DoOnline(IotRequest req)
		{
			api.Dispatch(Name, req.Device.Mac, $"ONLINE-{req.Command.Name}", "");
		}

		private void DoLogout(IotDevice device)
		{
			api.Dispatch(Name, device.Mac, "LOGOUT", "");
		}

		private void DoUpload(IotRequest req)
		{
			api.Dispatch(Name, req.Device.Mac, $"UPLOAD-{req.Command.Name}", req.Command.DataHex);
		}

		private void DoUpData(IotRequest req)
		{
			api.Dispatch(Name, req.Device.Mac, $"UPDATA-{req.Command.Name}", req.Command.DataHex);
		}

		private void IotServer_NewSessionConnected(IotSession session)
		{
			logger.Debug($"IoT-Session-BGN: {session.RemoteEndPoint}");
		}

		private void IotServer_SessionClosed(IotSession session, CloseReason value)
		{
			if (session.Device != null)
			{
				DoLogout(session.Device);
            }

            logger.Debug($"IoT-Session-END: {session.RemoteEndPoint}");
        }

        protected override void OnStarted()
		{
			api = Bootstrap.GetServerByName("ApiServer") as IDispatcher;
            interval = Config.Options.GetValue<int>("uploadInterval", 30);
            ConnectToApiClient();

			logger.Debug($"IoT-Socket-START: {Config.Ip}:{Config.Port}");
		}

		protected override void OnStopped()
		{
			logger.Debug($"IoT-Socket-STOP: {Config.Ip}:{Config.Port}");
		}
	}
}

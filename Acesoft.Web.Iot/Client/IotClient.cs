using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using SuperSocket.ClientEngine;
using Acesoft.Config;
using Acesoft.Web.IoT.Models;
using Acesoft.Web.IoT.Config;
using Acesoft.Web.Multitenancy;
using System.Collections.Concurrent;

namespace Acesoft.Web.IoT.Client
{
    public class IotClient : IIotClient
    {
        private readonly ConcurrentDictionary<string, IotContext> actions;

        private readonly ILogger<IotClient> logger;
        private readonly Tenant tenant;
        private readonly IotAccess access;
        private readonly IIotService iotService;
        private readonly ICacheService cacheService;

        public EasyClient Client { get; private set; }

        public IotClient(ILogger<IotClient> logger, IIotService iotService, ICacheService cacheService)
        {
            this.logger = logger;
            this.iotService = iotService;
            this.cacheService = cacheService;

            this.actions = new ConcurrentDictionary<string, IotContext>();
            this.tenant = App.Context.GetTenantContext().Tenant;
            this.access = ConfigContext.GetConfig<IotConfig>(tenant.Name).Servers["iot"];

            this.Client = new EasyClient();
            this.Client.Initialize(new IotReceiveFilter(), this.OnReceived);
            this.Client.Closed += (s, e) =>
            {
                if (access.ReconnectClose)
                {
                    var t1 = Open();
                }
            };

            // open
            var t2 = Open();
        }

        #region open&close
        public async Task Open()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(access.ApiServerIp), access.ApiServerPort);
            if (await Client.ConnectAsync(endpoint))
            {
                logger.LogDebug("IotClient has connected to server!");

                // 发送握手协议
                await Send(new IotRequest
                {
                    Tenant = tenant.Name,
                    Mac = App.IdWorker.NextStringId(),
                    Cmd = "CONN",
                    Body = ""
                });
            }
            else
            {
                logger.LogWarning("IotClient has't connected to server!");

                if (access.ReconnectFail)
                {
                    await Task.Delay(access.ConnectInterval);
                    var t = Open();
                }
            }
        }

        public void Close()
        {
            this.Client.Close();
            logger.LogDebug("IotClient has closed!");
        }
        #endregion

        #region send&receive
        public async Task<IotContext> Send(IotRequest request)
        {
            if (!Client.IsConnected)
            {
                await Open();
            }

            var ctx = new IotContext(request);
            request.Tenant = this.tenant.Name;
            if (!actions.TryAdd(request.Mac, ctx))
            {
                throw new AceException("不能频繁发送操作请求！");
            }

            logger.LogDebug($"IotClient-SEND: {request.Tenant}-{request.Mac}-{request.Cmd} {request.Body}");
            await ctx.Send(Client);
            ctx.Wait.WaitOne(access.CmdTimeout);

            if (ctx.Response != null)
            {
                return ctx;
            }

            actions.TryRemove(request.Mac, out ctx);
            throw new AceException("操作超时，请检查网络是否连接！");
        }

        private async void OnReceived(IotRequest res)
        {
            logger.LogDebug($"IotClient-RECE: {res.Tenant}-{res.Mac}-{res.Cmd} {res.Body}");
            try
            {
                var cmd = res.Cmd.Split('-')[0];
                switch (cmd)
                {
                    case "BACK":
                        if (actions.TryRemove(res.Mac, out IotContext ctx))
                        {
                            ctx.Response = res;
                            ctx.Wait.Set();
                        }
                        break;

                    case "LOGIN":
                        await iotService.Login(res.Mac, res.Body);
                        break;

                    case "LOGOUT":
                        await iotService.Logout(res.Mac);
                        break;

                    case "UPLOAD":
                        await iotService.Upload(res.Mac, res.Body);
                        break;
                    
                    case "ONLINE":
                        await iotService.Online(res.Mac);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"IotClient-ERRR: {res.Tenant}-{res.Mac}-{res.Cmd} {res.Body} {ex.GetMessage()}");
            }
            finally
            {
                if (iotService.NeedCloseSession)
                {
                    iotService.Session.Dispose();
                }
                if (cacheService.NeedCloseSession)
                {
                    cacheService.Session.Dispose();
                }
            }
        }
        #endregion
    }
}

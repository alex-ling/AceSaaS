using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Acesoft.Data;
using Acesoft.Web.Cloud;
using Acesoft.Web.IoT.Hubs;
using Acesoft.Web.IoT.Models;
using Acesoft.Web.IoT.Entity;
using Acesoft.Util;

namespace Acesoft.Web.IoT.Services
{
    public class IotService : StoreServiceBase, IIotService
    {
        private readonly ICloudService cloudService;
        private readonly ICacheService cacheService;
        private readonly IHubContext<IotDataHub> iotDataHub;
        private readonly ILogger<IotService> logger;

        public IotService(ILogger<IotService> logger, 
            IStore store,
            ICloudService cloudService, 
            ICacheService cacheService,
            IHubContext<IotDataHub> iotDataHub) : base(store)
        {
            this.logger = logger;
            this.cloudService = cloudService;
            this.cacheService = cacheService;
            this.iotDataHub = iotDataHub;
        }

        #region device
        public IotDevice GetDevice(string mac, long userId)
        {
            var device = cacheService.GetDevice(mac);
            if (device.Owner_id != userId)
            {
                throw new AceException("用户不是设备的拥有者");
            }
            return device;
        }

        public IEnumerable<IotData> GetDevices(long userId)
        {
            var ctx = new RequestContext("iot", "iot_get_devices")
                .SetCmdType(CmdType.query)
                .SetParam(new { userId });
            return Session.Query(ctx).Select(item => GetData((string)item.mac).Do(d => d.Write = item.write));
        }
        #endregion

        #region login
        public async Task Login(string mac, string body)
        {
            var iotData = GetData(mac);
            iotData.Online = true;
            iotData.LastLoginTime = DateTime.Now;
            iotData.Save();

            await iotDataHub.Clients.Group(mac).SendAsync("Send", iotData);

            var ctx = new RequestContext("iot", "exe_iot_login")
                .SetParam(new
                {
                    mac,
                    softver = NaryHelper.YmHexToDouble(body.Substring(0, 4)),
                    hardver = NaryHelper.YmHexToDouble(body.Substring(4, 4)),
                    wifi = EncodingHelper.HexToBytes(body.Substring(8)).ToStr("")
                });
            await Session.ExecuteAsync(ctx);

            var json = SerializeHelper.ToJson(ctx.Params);
            logger.LogDebug($"IotService-LOGIN: {json}");
            LogRec(mac, iotData.Device.Sbno, "登录", "Login", body, json);
        }
        #endregion

        #region online
        public async Task Online(string mac)
        {
            var iotData = GetData(mac);
            iotData.Online = true;
            iotData.LastCollectTime = DateTime.Now;
            iotData.Save();

            await iotDataHub.Clients.Group(mac).SendAsync("Send", iotData);

            var ctx = new RequestContext("iot", "exe_iot_online")
                .SetParam(new
                {
                    mac
                });
            await Session.ExecuteAsync(ctx);

            var json = SerializeHelper.ToJson(ctx.Params);
            logger.LogDebug($"IotService-LOGIN: {json}");
            LogRec(mac, iotData.Device.Sbno, "上线", "Online", null, json);
        }
        #endregion

        #region upload
        public async Task Upload(string mac, string body)
        {
            var iotData = GetData(mac, true);
            if (!iotData.Online)
            {
                await Online(mac);
            }
            iotData.Online = true;
            iotData.LastCollectTime = DateTime.Now;
            foreach (var param in iotData.Device.Params)
            {
                iotData.Values[param.Key] = GetParamValue(body, param.Value);
            }
            await iotDataHub.Clients.Group(mac).SendAsync("Send", iotData);

            // save data to db first
            if (iotData.IsNeedSave())
            {
                await Session.ExecuteAsync(iotData.Device.GetInsertSql(), iotData.GetSaveParams());
            }

            // save data to redis last
            iotData.Save();

            var json = SerializeHelper.ToJson(iotData.Values);
            logger.LogDebug($"IotService-LOGIN: {json}");
            LogRec(mac, iotData.Device.Sbno, "上传", "Upload", body, json);
        }

        private object GetParamValue(string dataHex, IotParam param)
        {
            try
            {
                var length = param.Type == "C" ? 1 : param.Length;
                var val = dataHex.Substring(2 * param.Start, 2 * length);
                if (val != "EEEE" && val != "EE")
                {
                    switch (param.Type)
                    {
                        case "I":
                            return NaryHelper.YmHexToInt(val);
                        case "B":
                            return NaryHelper.HexToInt(val) > 0;
                        case "C":
                            return BinaryHelper.GetHexBit(val, param.Length);
                        default:
                            return NaryHelper.YmHexToDouble(val);
                    }
                }
                return "异常";
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region logout
        public async Task Logout(string mac)
        {
            var iotData = GetData(mac);
            iotData.Online = false;
            iotData.Save();

            // set clients.
            await iotDataHub.Clients.Group(mac).SendAsync("Send", iotData);

            var ctx = new RequestContext("iot", "exe_iot_logout")
                .SetParam(new
                {
                    mac
                });
            await Session.ExecuteAsync(ctx);

            var json = SerializeHelper.ToJson(ctx.Params);
            logger.LogDebug($"IotService-LOGIN: {json}");
            LogRec(mac, iotData.Device.Sbno, "下线", "Logout", null, json);
        }
        #endregion

        #region iotlog
        private void LogRec(string mac, string sbno, string name, string cmd, string body, string json)
        {
            if (cacheService.Get("enable_reclog", false))
            {
                var rec = new IoT_RecLog();
                rec.InitializeId();
                rec.DCreate = DateTime.Now;
                rec.Mac = mac;
                rec.Sbno = sbno;
                rec.Name = name;
                rec.Cmd = cmd;
                rec.Body = body;
                rec.Json = json;
                Session.Insert(rec);
            }
        }

        public void LogOpt(IoT_OptLog opt)
        {
            if (cacheService.Get("enable_optlog", false))
            {
                Session.Insert(opt);
            }
        }
        #endregion

        #region data
        public IotData GetData(string mac, bool fetchWeather = false)
        {
            var data = cacheService.GetData(mac);

            if (fetchWeather && data.Device.Location.CityId.HasValue())
            {
                data.Weather = cloudService.GetWeatherService().GetCachedWeaAqi(data.Device.Location.CityId);
            }

            return data;
        }
        #endregion

        #region owner
        public void PutOwner(object param)
        {
            var ctx = new RequestContext("iot", "exe_iot_owner")
                .SetParam(param);
            Session.Execute(ctx);
        }
        #endregion

        #region bind
        public void PostBind(object param)
        {
            var ctx = new RequestContext("iot", "exe_iot_bind")
                .SetParam(param);
            Session.Execute(ctx);
        }

        public void DeleteBind(object param)
        {
            var ctx = new RequestContext("iot", "exe_iot_unbind")
                .SetParam(param);
            Session.Execute(ctx);
        }
        #endregion

        #region auth
        public void PostAuth(object param)
        {
            var ctx = new RequestContext("iot", "exe_iot_auth")
                .SetParam(param);
            Session.Execute(ctx);
        }

        public void PutAuth(object param)
        {
            var ctx = new RequestContext("iot", "userdevice")
                .SetCmdType(CmdType.update)
                .SetParam(param);
            Session.Execute(ctx);
        }

        public void DeleteAuth(long id)
        {
            var ctx = new RequestContext("iot", "userdevice")
                .SetCmdType(CmdType.delete)
                .SetParam(new { id, ids = new long[] { id } });
            Session.Execute(ctx);
        }
        #endregion

        #region reset
        public void Reset(string mac)
        {
            var ctx = new RequestContext("iot", "exe_iot_reset")
                .SetParam(new
                {
                    mac
                });
            Session.Execute(ctx);
        }
        #endregion
    }
}
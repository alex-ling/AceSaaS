using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Acesoft.Cache;
using Acesoft.Data;
using Acesoft.Web.Cloud;
using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT.Services
{
    public class IotService : ServiceBase, IIotService
    {
        private readonly ICloudService cloudService;
        private readonly ICacheService cacheService;

        public IotService(ICloudService cloudService, ICacheService cacheService)
        {
            this.cloudService = cloudService;
            this.cacheService = cacheService;
        }

        #region device
        public IotDevice GetDevice(string mac)
        {
            return cacheService.GetDevice(mac);
        }

        public IotDevice GetDevice(string mac, long userId)
        {
            var device = GetDevice(mac);
            if (device.Owner_id != userId)
            {
                throw new AceException("用户不是设备的拥有者");
            }
            return device;
        }

        public IEnumerable<dynamic> GetDevices(long userId)
        {
            var ctx = new RequestContext("iot", "iot_get_devices")
                .SetCmdType(CmdType.query)
                .SetParam(new { userId });
            var res = Session.Query(ctx);

            // set online & values from cache.
            foreach (var item in res)
            {
                var data = GetData(item.mac);
                item.online = data.Online;
                item.values = data.Values;
            }

            return res; 
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
                .SetParam(new { id });
            Session.Execute(ctx);
        }
        #endregion

        #region reset
        public void Reset(string mac)
        {
            var ctx = new RequestContext("iot", "exec_iot_reset")
                .SetParam(new
                {
                    mac
                });
            Session.Execute(ctx);
        }
        #endregion
    }
}
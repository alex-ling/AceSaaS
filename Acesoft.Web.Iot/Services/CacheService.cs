using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Cache;
using Acesoft.Data;
using Acesoft.Web.Cloud;
using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT.Services
{
    public class CacheService : StoreServiceBase, ICacheService
    {
        public CacheService(IStore store) : base(store)
        { }

        public T Get<T>(string key, T defaultValue)
        {
            return App.Cache.GetOrAdd($"iot_set_{key}", k => defaultValue);
        }

        public void Set<T>(string key, T value)
        {
            App.Cache.Set($"iot_set_{key}", value);
        }

        public string GetMac(string sbno)
        {
            return App.Cache.GetOrAdd($"iot_mac_{sbno}",
                key =>
                {
                    return Session.ExecuteScalar<string>(
                        new RequestContext("iot", "iot_get_mac")
                        .SetParam(new
                        {
                            sbno
                        })
                    );
                }
            );
        }

        public IotData GetData(string mac)
        {
            return App.Cache.GetOrAdd($"iot_data_{mac}",
                key =>
                {
                    var data = new IotData
                    {
                        Device = GetDevice(mac)
                    };

                    // set param default with null
                    foreach (var param in data.Device.Params.Keys)
                    {
                        data.Values[param] = null;
                    }

                    return data;
                }
            )
            .Do(d => d.Device = GetDevice(mac));
        }

        public IotDevice GetDevice(string mac)
        {
            var device = App.Cache.GetOrAdd($"iot_device_{mac}",
                key =>
                {
                    var d = Session.QuerySingle(
                        new RequestContext("iot", "iot_get_device")
                        .SetCmdType(CmdType.query)
                        .SetParam(new
                        {
                            mac
                        })
                    );

                    return new IotDevice
                    {
                        Id = d.id,
                        Alias = d.alias,
                        Cpno = d.cpno,
                        Cpxh = d.cpxh,
                        Mac = d.mac,
                        Name = d.name,
                        NetAlert = d.netalert,
                        Owner_id = d.owner_id,
                        Owner_phone = d.owner_phone,
                        Sbno = d.sbno,
                        Location = new Location
                        {
                            AdCode = d.adcode,
                            Address = d.address,
                            City = d.city,
                            CityId = d.cityid,
                            District = d.district,
                            Latitude = d.latitude,
                            Longitude = d.longitude,
                            Province = d.province
                        }
                    };
                }
            );

            // load params & cmds from cache.
            var cpno = device.Sbno.Substring(0, 4);
            device.Params = GetParams(cpno);
            device.Cmds = GetCmds(cpno);

            return device;
        }

        public IDictionary<string, IotParam> GetParams(string cpno)
        {
            return App.Cache.GetOrAdd($"iot_params_{cpno}",
                key =>
                {
                    return Session.Query<IotParam>(
                        new RequestContext("iot", "iot_get_params")
                        .SetCmdType(CmdType.query)
                        .SetParam(new
                        {
                            cpno
                        })
                    ).ToDictionary(p => p.Name);
                }
            );
        }

        public IDictionary<string, IotCmd> GetCmds(string cpno)
        {
            var cmds = App.Cache.GetOrAdd($"iot_cmds_{cpno}",
                key =>
                {
                    return Session.Query<IotCmd>(
                        new RequestContext("iot", "iot_get_cmds")
                        .SetCmdType(CmdType.query)
                        .SetParam(new
                        {
                            cpno
                        })
                    ).ToDictionary(p => p.Cmd);
                }
            );

            // add common cmd for every device
            cmds.Add("00F2", new IotCmd
            {
                Cmd = "00F2",
                Name = "设置上传周期"
            });
            cmds.Add("00F3", new IotCmd
            {
                Cmd = "00F2",
                Name = "开启实时上传"
            });

            return cmds;
        }

        public void RemoveData(string mac)
        {
            App.Cache.Remove($"iot_data_{mac}");
        }

        public void RemoveDevice(string mac)
        {
            App.Cache.Remove($"iot_device_{mac}");
        }

        public void RemoveParams(string cpno)
        {
            App.Cache.Remove($"iot_params_{cpno}");
        }

        public void RemoveCmds(string cpno)
        {
            App.Cache.Remove($"iot_cmds_{cpno}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT
{
    public interface IIotService
    {
        IotDevice GetDevice(string mac);
        IotDevice GetDevice(string mac, long userId);
        IotData GetData(string mac, bool fetchWeather = false);
        IEnumerable<dynamic> GetDevices(long userId);

        void PutOwner(object param);
        void PostBind(object param);
        void DeleteBind(object param);
        void PostAuth(object param);
        void PutAuth(object param);
        void DeleteAuth(long id);
        void Reset(string mac);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Web.IoT.Entity;
using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT
{
    public interface IIotService : IStoreServiceBase
    {
        IotDevice GetDevice(string mac, long userId);
        IotData GetData(string mac, bool fetchWeather = false);
        IEnumerable<IotData> GetDevices(long userId);

        Task Login(string mac, string body);
        Task Online(string mac);
        Task Upload(string mac, string body);
        Task Logout(string mac);

        void LogOpt(IoT_OptLog opt);

        void PutOwner(object param);
        void PostBind(object param);
        void DeleteBind(object param);
        void PostAuth(object param);
        void PutAuth(object param);
        void DeleteAuth(long id);
        void Reset(string mac);
    }
}

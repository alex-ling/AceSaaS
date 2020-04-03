using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT
{
    public interface ICacheService : IStoreServiceBase
    {
        T Get<T>(string key, T defaultValue);
        void Set<T>(string key, T value);

        string GetMac(string sbno);
        IotData GetData(string mac);
        IotDevice GetDevice(string mac);
        IDictionary<string, IotParam> GetParams(string cpno);
        IDictionary<string, IotCmd> GetCmds(string cpno);
        void RemoveMac(string sbno);
        void RemoveData(string mac);
        void RemoveDevice(string mac);
        void RemoveParams(string cpno);
        void RemoveCmds(string cpno);
    }
}

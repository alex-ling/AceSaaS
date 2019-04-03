using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.IoT.Models;

namespace Acesoft.Web.IoT
{
    public interface ICacheService
    {
        T Get<T>(string key, T defaultValue);
        void Set<T>(string key, T value);

        IotData GetData(string mac);
        IotDevice GetDevice(string mac);
        IDictionary<string, IotParam> GetParams(string cpno);
        IDictionary<string, IotCmd> GetCmds(string cpno);
        void RemoveData(string mac);
        void RemoveDevice(string mac);
        void RemoveParams(string cpno);
        void RemoveCmds(string cpno);
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Acesoft.Util
{
    public static class ConfigHelper
    {
        public static T GetAppSetting<T>(string key)
        {
            return ConfigurationManager.AppSettings.GetValue<T>(key);
        }

        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            return ConfigurationManager.AppSettings.GetValue<T>(key, defaultValue);
        }
    }
}

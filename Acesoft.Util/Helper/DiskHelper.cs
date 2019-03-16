using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Acesoft.Util
{
    public static class DiskHelper
    {
        public static string OpenDriveProperty(string key, string disk = null)
        {
            if (!disk.HasValue())
            {
                disk = AppDomain.CurrentDomain.BaseDirectory.Substring(0, 2);
            }
            var mo = new ManagementObject($"win32_logicaldisk.deviceid=\"{disk}\"");
            mo.Get();
            return mo[key].ToString();
        }
    }
}

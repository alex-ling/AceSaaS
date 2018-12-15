using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace Acesoft.Util
{
    public static class RegistryHelper
    {
        public static RegistryKey OpenHLM(string key)
        {
            return Registry.LocalMachine.OpenSubKey(key);
        }

        public static RegistryKey OpenHCU(string key)
        {
            return Registry.CurrentUser.OpenSubKey(key);
        }

        public static RegistryKey OpenHCR(string key)
        {
            return Registry.ClassesRoot.OpenSubKey(key);
        }

        public static RegistryKey OpenHU(string key)
        {
            return Registry.Users.OpenSubKey(key);
        }
    }
}

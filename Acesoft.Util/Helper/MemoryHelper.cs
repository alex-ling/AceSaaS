using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Acesoft.Util
{
    public class MemoryHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUS
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint length;
            public uint memoryLoad;
            public ulong totalPhys;
            public ulong availPhys;
            public ulong totalPageFile;
            public ulong availPageFile;
            public ulong totalVirtual;
            public ulong availVirtual;
            public ulong availExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.length = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORYSTATUS meminfo);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public static MEMORYSTATUS GetMemoryStatus()
        {
            var memory = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memory);
            return memory;
        }

        public static MEMORYSTATUSEX GetMemoryStatusEx()
        {
            var memory = new MEMORYSTATUSEX();
            GlobalMemoryStatusEx(memory);
            return memory;
        }
    }
}

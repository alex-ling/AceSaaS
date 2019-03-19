using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Acesoft.Util
{
    public static class ProcessHelper
    {
        public static void StartProcess(string filePath, string args, TextWriter output = null)
        {
            var processStart = new ProcessStartInfo(filePath)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Verb = "runas",
                Arguments = args
            };

            var process = Process.Start(processStart);
            using (var sr = process.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                    (output ?? Console.Out).WriteLine(sr.ReadLine());
                }
            }

            if (!process.HasExited)
            {
                process.WaitForExit();
                process.Kill();
            }
        }
    }
}

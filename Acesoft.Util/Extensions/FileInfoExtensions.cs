using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acesoft
{
    public static class FileInfoExtensions
    {
        public static string Read(this FileInfo file)
        {
            using (var rd = file.OpenText())
            {
                return rd.ReadToEnd();
            }
        }

        public static void Write(this FileInfo file, string content)
        {
            using (var wr = new StreamWriter(file.FullName, false, Encoding.UTF8))
            {
                wr.Write(content);
            }
        }
    }
}

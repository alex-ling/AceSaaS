using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acesoft.Util
{
    public static class DirectoryHelper
    {
        public static void CopyTo(this DirectoryInfo dir, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in dir.GetFiles())
            {
                file.CopyTo(Path.Combine(path, file.Name));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acesoft.Util
{
    public static class FileHelper
    {
        #region read
        public static string Read(this FileInfo file)
        {
            using (var rd = file.OpenText())
            {
                return rd.ReadToEnd();
            }
        }

        public static string Read(string fileName)
        {
            using (var rd = File.OpenText(fileName))
            {
                return rd.ReadToEnd();
            }
        }

        public static string Read(string fileName, string encoding)
        {
            using (var rd = new StreamReader(fileName, Encoding.GetEncoding(encoding)))
            {
                return rd.ReadToEnd();
            }
        }

        public static Stream ReadStream(string fileName)
        {
            return File.OpenRead(fileName);
        }
        #endregion

        #region write
        public static void Write(this FileInfo file, string content)
        {
            using (var wr = new StreamWriter(file.FullName, false, Encoding.UTF8))
            {
                wr.Write(content);
            }
        }

        public static void Write(string fileName, string content)
        {
            TextWriter writer = File.CreateText(fileName);
            try
            {
                writer.Write(content);
            }
            finally
            {
                writer.Close();
            }
        }

        public static void Write(string fileName, string str, string encoding)
        {
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(encoding));
            try
            {
                writer.Write(str);
            }
            finally
            {
                writer.Close();
            }
        }

        public static void Write(string fileName, Stream stream)
        {
            int length = 256;
            byte[] buffer = new byte[length];
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                int bytesRead = stream.Read(buffer, 0, length);
                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);
                }
            }
        }

        public static void Write(string fileName, byte[] data)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }
        }
        #endregion

        #region delete
        public static void Delete(string file)
        {
            File.Delete(file);
        }
        #endregion

        #region convert
        public static byte[] ConvertToByte(Stream input, string fileExt)
        {
            int length = (int)input.Length;
            byte[] val = new byte[length + 5];

            input.Read(val, 0, length);
            input.Close();

            for (int i = 0; i < 5; i++)
            {
                char[] c = new char[] { ' ' };

                if (i < fileExt.Length)
                {
                    c = fileExt.Substring(i, 1).ToCharArray();
                }

                val[length + i] = (byte)c[0];
            }

            return val;
        }

        public static string ConvertToFile(byte[] value, string path)
        {
            int length = value.GetLength(0);
            string fileExt = string.Empty;
            string file = path;
            FileStream fs = null;

            for (int i = 0; i < 5; i++)
            {
                char c = (char)value[length - 5 + i];
                fileExt += c.ToString();
            }

            fileExt = fileExt.Trim();
            file += fileExt;

            try
            {
                fs = File.Create(file);
                fs.Write(value, 0, length - 5);
                fs.Flush();

                return fileExt;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }
        #endregion
    }
}

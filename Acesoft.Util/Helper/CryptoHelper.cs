using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Acesoft.Util
{
    public static class CryptoHelper
    {
        //sha1
        public static string ComputeSHA1Signature(string key, string data)
        {
            using (var algorithm = new HMACSHA1(EncodingHelper.ToBytes(key)))
            {
                return algorithm.ComputeHash(EncodingHelper.ToBytes(data)).ToBase64();
            }
        }

        //md5
        public static string ComputeMD5(string key, string data)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.ASCII.GetBytes($"{data}:{key}");
            return md5.ComputeHash(bytes).ToBase64();
        }
    }
}

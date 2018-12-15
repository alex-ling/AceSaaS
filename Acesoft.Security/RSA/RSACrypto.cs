using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Caching.Distributed;

using Acesoft.Cache;
using Acesoft.Util;

namespace Acesoft.Security
{
    public class RSACrypto : IRSACrypto
    {
        private RSACryptoServiceProvider _RSA;

        public string RSAKey { get; private set; }
        public RSAData RSAData { get; private set; }
        public RSAParameters RSAParam { get; private set; }

        public RSACrypto(string key = null)
        {
            _RSA = new RSACryptoServiceProvider();

            if (key.HasValue())
            {
                RSAKey = key;
                _RSA.FromXmlString(CacheContext.Cache.GetString(RSAKey));
            }
            else
            {
                RSAKey = "rsa_" + Guid.NewGuid().ToString("n");
                CacheContext.Cache.SetString(RSAKey, _RSA.ToXmlString(true));
            }

            RSAParam = _RSA.ExportParameters(true);
            RSAData = new RSAData
            {
                Key = RSAKey,
                E = RSAParam.Exponent.ToHex(),
                M = RSAParam.Modulus.ToHex()
            };
        }

        public string Encrypt(string code)
        {
            var bytes = Encoding.ASCII.GetBytes(EncodingHelper.ToBase64(code));
            return _RSA.Encrypt(bytes, false).ToHex();
        }

        public string Decrypt(string code)
        {
            var rv = _RSA.Decrypt(EncodingHelper.HexToBytes(code), false);
            return EncodingHelper.FromBase64(Encoding.ASCII.GetString(rv));
        }
    }
}

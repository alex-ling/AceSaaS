using System;
using System.Collections.Generic;
using System.Text;
using Acesoft.Security;

namespace Acesoft.Rbac.Services
{
    public class RsaService : IRsaService
    {
        public string Decrypt(string key, string data)
        {
            var rsa = new RSACrypto(key);
            return rsa.Decrypt(data);
        }

        public string Encrypt(string key, string data)
        {
            var rsa = new RSACrypto(key);
            return rsa.Encrypt(data);
        }

        public RSAData GetRsa()
        {
            var rsa = new RSACrypto();
            return rsa.RSAData;
        }
    }
}

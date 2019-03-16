using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Security;

namespace Acesoft.Rbac
{
    public interface IRsaService
    {
        RSAData GetRsa();
        string Encrypt(string key, string data);
        string Decrypt(string key, string data);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Security
{
    public interface IRSACrypto
    {
        string RSAKey { get; }
        RSAData RSAData { get; }
        string Encrypt(string data);
        string Decrypt(string data);
    }
}

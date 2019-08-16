using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Security
{
    public interface IByteCrypto
    {
        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] bytes);
    }
}

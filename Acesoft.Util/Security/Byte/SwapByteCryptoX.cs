﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Security
{
    public class SwapByteCryptoX : IByteCrypto
    {
        public byte[] Decrypt(byte[] bytes)
        {
            var len = bytes.Length;
            var bs = new byte[len];
            for (var i = 0; i < len; i++)
            {
                bs[len - i - 1] = Decrypt(bytes[i], len - i);
            }
            return bs;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            var len = bytes.Length;
            var bs = new byte[len];
            for (var i = 0; i < len; i++)
            {
                bs[len - i - 1] = Encrypt(bytes[i], i + 1);
            }
            return bs;
        }

        private byte Encrypt(byte b, int key)
        {
            var m = key & 7;
            b ^= 0xdb;
            return (byte)(key ^ ((b << m) | (b >> (8 - m))));
        }

        public static byte Decrypt(byte b, int key)
        {
            var m = key & 7;
            b ^= (byte)key;
            return (byte)(0xdb & ((b >> m) | (b << (8 - m))));
        }
    }
}

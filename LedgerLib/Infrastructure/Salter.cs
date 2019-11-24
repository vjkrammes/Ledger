using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace LedgerLib.Infrastructure
{
    public static class Salter
    {
        public static byte[] GeneratSalt(int length)
        {
            byte[] ret = new byte[length];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(ret);
            return ret;
        }
    }
}

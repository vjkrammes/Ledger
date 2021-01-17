using System.Security.Cryptography;

namespace LedgerLib.Infrastructure
{
    public static class Salter
    {
        public static byte[] GeneratSalt(int length)
        {
            var ret = new byte[length];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(ret);
            return ret;
        }
    }
}

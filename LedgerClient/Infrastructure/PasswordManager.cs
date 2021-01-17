using LedgerClient.Interfaces;

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LedgerClient.Infrastructure
{
    public class PasswordManager : IPasswordManager
    {
        private readonly byte[] _key;            // 256 bits
        private readonly byte[] _iv;             // 128 bits
        private readonly Dictionary<int, byte[]> _passwords = new Dictionary<int, byte[]>();

        public PasswordManager()
        {
            _key = new byte[256 / 8];
            _iv = new byte[128 / 8];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_key);
            rng.GetBytes(_iv);
        }

        public void Set(string pw, int index)
        {
            var pwbytes = Encoding.UTF8.GetBytes(pw);
            using var key = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using var encryptor = key.CreateEncryptor(_key, _iv);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(pwbytes, 0, pwbytes.Length);
            cs.FlushFinalBlock();
            _passwords[index] = ms.ToArray();
        }

        public string Get(int index)
        {
            if (_passwords.Count == 0 || !_passwords.ContainsKey(index))
            {
                return string.Empty;
            }
            using var key = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using var decryptor = key.CreateDecryptor(_key, _iv);
            using var ms = new MemoryStream(_passwords[index]);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            var ptbytes = new byte[_passwords[index].Length];
            var dbc = cs.Read(ptbytes, 0, ptbytes.Length);
            return Encoding.UTF8.GetString(ptbytes, 0, dbc);
        }
    }
}

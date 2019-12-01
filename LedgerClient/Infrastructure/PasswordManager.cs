using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using LedgerClient.Interfaces;

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
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_key);
            rng.GetBytes(_iv);
        }

        public void Set(string pw, int index)
        {
            byte[] pwbytes = Encoding.UTF8.GetBytes(pw);
            using RijndaelManaged key = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using ICryptoTransform encryptor = key.CreateEncryptor(_key, _iv);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
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
            using RijndaelManaged key = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using ICryptoTransform decryptor = key.CreateDecryptor(_key, _iv);
            using MemoryStream ms = new MemoryStream(_passwords[index]);
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            byte[] ptbytes = new byte[_passwords[index].Length];
            int dbc = cs.Read(ptbytes, 0, ptbytes.Length);
            return Encoding.UTF8.GetString(ptbytes, 0, dbc);
        }
    }
}

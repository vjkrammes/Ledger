using LedgerClient.Interfaces;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LedgerClient.Infrastructure
{
    public class StringCypherService : IStringCypherService
    {
        // This constant string is used as a "salt" value for the CreateEncryptor function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] _initVectorBytes = Encoding.ASCII.GetBytes(":L3f%h@L3dfQo5$z");

        // this constant is used to determine the key size of the encryption algorithm
        private const int _keysize = 256;

        public string Encrypt(string plaintext, string passphrase, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return string.Empty;
            }

            var ptbytes = Encoding.UTF8.GetBytes(plaintext);
            using var password = new PasswordDeriveBytes(passphrase, salt);
            var keybytes = password.GetBytes(_keysize / 8);
            using var symmetrickey = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using var encryptor = symmetrickey.CreateEncryptor(keybytes, _initVectorBytes);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(ptbytes, 0, ptbytes.Length);
            cs.FlushFinalBlock();
            var ctbytes = ms.ToArray();
            return Convert.ToBase64String(ctbytes);
        }

        public string Decrypt(string ciphertext, string passphrase, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                return string.Empty;
            }

            var ctbytes = Convert.FromBase64String(ciphertext);
            using var password = new PasswordDeriveBytes(passphrase, salt);
            var keybytes = password.GetBytes(_keysize / 8);
            using var symmetrickey = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using var decryptor = symmetrickey.CreateDecryptor(keybytes, _initVectorBytes);
            using var ms = new MemoryStream(ctbytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            var ptbytes = new byte[ctbytes.Length];
            var dbc = cs.Read(ptbytes, 0, ptbytes.Length);
            return Encoding.UTF8.GetString(ptbytes, 0, dbc);
        }
    }
}

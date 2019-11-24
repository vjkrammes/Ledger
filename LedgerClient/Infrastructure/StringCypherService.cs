using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using LedgerClient.Interfaces;

namespace LedgerClient.Infrastructure
{
    public class StringCypherService : IStringCypherService
    {
        // This constant string is used as a "salt" value for the CreateEncryptor function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes(":L3f%h@L3dfQo5$z");

        // this constant is used to determine the key size of the encryption algorithm
        private const int keysize = 256;

        public string Encrypt(string plaintext, string passphrase, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(plaintext))
                return string.Empty;
            byte[] ptbytes = Encoding.UTF8.GetBytes(plaintext);
            using PasswordDeriveBytes password = new PasswordDeriveBytes(passphrase, salt);
            byte[] keybytes = password.GetBytes(keysize / 8);
            using RijndaelManaged symmetrickey = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using ICryptoTransform encryptor = symmetrickey.CreateEncryptor(keybytes, initVectorBytes);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(ptbytes, 0, ptbytes.Length);
            cs.FlushFinalBlock();
            byte[] ctbytes = ms.ToArray();
            return Convert.ToBase64String(ctbytes);
        }

        public string Decrypt(string ciphertext, string passphrase, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;
            byte[] ctbytes = Convert.FromBase64String(ciphertext);
            using PasswordDeriveBytes password = new PasswordDeriveBytes(passphrase, salt);
            byte[] keybytes = password.GetBytes(keysize / 8);
            using RijndaelManaged symmetrickey = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            using ICryptoTransform decryptor = symmetrickey.CreateDecryptor(keybytes, initVectorBytes);
            using MemoryStream ms = new MemoryStream(ctbytes);
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            byte[] ptbytes = new byte[ctbytes.Length];
            int dbc = cs.Read(ptbytes, 0, ptbytes.Length);
            return Encoding.UTF8.GetString(ptbytes, 0, dbc);
        }
    }
}

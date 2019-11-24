namespace LedgerClient.Interfaces
{
    public interface IStringCypherService
    {
        string Encrypt(string plaintext, string passphrase, byte[] salt = null);
        string Decrypt(string cyphertext, string passphrase, byte[] salt = null);
    }
}

using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;

using NUnit.Framework;

namespace Ledger.Tests
{
    [TestFixture]
    class StringCypherTest
    {
        [Test]
        public void CypherTestWithoutSalt()
        {
            IStringCypherService cypher = new StringCypherService();
            string password = "Te3tPa$sw0rd";
            string plaintext = "This is a test of the cypher service";
            string cyphertext = cypher.Encrypt(plaintext, password);
            string decrypted = cypher.Decrypt(cyphertext, password);
            Assert.IsTrue(decrypted == plaintext);
        }

        [Test]
        public void CypherTestWithSalt()
        {
            IStringCypherService cypher = new StringCypherService();
            byte[] salt = Tools.GenerateSalt(64);
            string password = "Te3tPa$sw0rd";
            string plaintext = "This is a test of the cypher service";
            string cyphertext = cypher.Encrypt(plaintext, password, salt);
            string decrypted = cypher.Decrypt(cyphertext, password, salt);
            Assert.IsTrue(decrypted == plaintext);
        }
    }
}

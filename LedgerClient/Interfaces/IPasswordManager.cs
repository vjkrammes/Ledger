namespace LedgerClient.Interfaces
{
    public interface IPasswordManager
    {
        void Set(string password);
        string Get();
    }
}

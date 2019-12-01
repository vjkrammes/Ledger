namespace LedgerClient.Interfaces
{
    public interface IPasswordManager
    {
        void Set(string password, int index);
        string Get(int index);
    }
}

using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IAccountTypeHistoryDAL : IHistoryDAL<AccountTypeHistoryEntity>
    {
        AccountTypeHistoryEntity Read(string description);
        AccountTypeHistoryEntity Read(int id);
    }
}

using System.Collections.Generic;

using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IAccountHistoryDAL : IHistoryDAL<AccountHistoryEntity>
    {
        IEnumerable<AccountHistoryEntity> GetForPayee(int pid);
        IEnumerable<AccountHistoryEntity> GetForAccountType(int atid);
        AccountHistoryEntity Read(int id);
    }
}

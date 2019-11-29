using System.Collections.Generic;

using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IAccountNumberHistoryDAL : IHistoryDAL<AccountNumberHistoryEntity>
    {
        IEnumerable<AccountNumberHistoryEntity> GetForAccount(int aid);
        AccountNumberHistoryEntity Current(int aid);
        AccountNumberHistoryEntity Read(int id);
    }
}

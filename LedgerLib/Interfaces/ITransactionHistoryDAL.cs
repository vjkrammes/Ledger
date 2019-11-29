using System.Collections.Generic;

using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface ITransactionHistoryDAL : IHistoryDAL<TransactionHistoryEntity>
    {
        IEnumerable<TransactionHistoryEntity> GetForAccount(int aid);
        TransactionHistoryEntity Read(int id);
    }
}

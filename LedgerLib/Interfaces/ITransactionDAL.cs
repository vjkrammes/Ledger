using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface ITransactionDAL : IDAL<TransactionEntity>
    {
        IEnumerable<TransactionEntity> GetForAccount(int aid);
        TransactionEntity Read(int id);
        decimal Total();
        decimal TotalForAccount(int aid);
        bool AccountHasTransactions(int aid);
    }
}
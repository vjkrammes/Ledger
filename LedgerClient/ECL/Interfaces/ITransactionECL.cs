using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface ITransactionECL : IECL<TransactionEntity, Transaction>
    {
        IEnumerable<Transaction> GetForAccount(int aid);
        Transaction Read(int id);
        decimal Total();
        decimal TotalForAccount(int aid);
        bool AccountHasTransactions(int aid);
    }
}

using System.Collections.Generic;

using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IAllotmentHistoryDAL : IHistoryDAL<AllotmentHistoryEntity>
    {
        IEnumerable<AllotmentHistoryEntity> GetForPool(int pid);
        IEnumerable<AllotmentHistoryEntity> GetForPayee(int pid);
        AllotmentHistoryEntity Read(int id);
    }
}

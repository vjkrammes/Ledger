using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IPoolHistoryDAL : IHistoryDAL<PoolHistoryEntity>
    {
        PoolHistoryEntity Read(int id);
        PoolHistoryEntity Read(string name);
    }
}
